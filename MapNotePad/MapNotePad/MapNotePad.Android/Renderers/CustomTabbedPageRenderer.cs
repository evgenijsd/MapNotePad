using Android.Content;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.Tabs;
using MapNotePad.Controls;
using MapNotePad.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace MapNotePad.Droid.Renderers
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        public CustomTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            this.InvertLayoutThroughScale();

            base.OnLayout(changed, l, t, r, b);
        }

        private void InvertLayoutThroughScale()
        {
            this.ViewGroup.ScaleY = -1;

            TabLayout tabLayout = null;
            ViewPager viewPager = null;

            for (int i = 0; i < ChildCount; ++i)
            {
                Android.Views.View view = GetChildAt(i);
                if (view is TabLayout tabs)
                    tabLayout = tabs;
                else if (view is ViewPager pager)
                    viewPager = pager;
            }

            tabLayout.ViewTreeObserver.AddOnGlobalLayoutListener(new GlobalLayoutListener(viewPager, tabLayout));

            tabLayout.ScaleY = viewPager.ScaleY = -1;
        }

        private class GlobalLayoutListener : Java.Lang.Object, Android.Views.ViewTreeObserver.IOnGlobalLayoutListener
        {
            private readonly ViewPager viewPager;
            private readonly TabLayout tabLayout;

            public GlobalLayoutListener(ViewPager viewPager, TabLayout tabLayout)
            {
                this.viewPager = viewPager;
                this.tabLayout = tabLayout;
            }

            public void OnGlobalLayout()
            {
                this.viewPager.SetPadding(0, -this.tabLayout.MeasuredHeight, 0, 0);
                this.tabLayout.ViewTreeObserver.RemoveOnGlobalLayoutListener(this);
            }
        }
    }
}