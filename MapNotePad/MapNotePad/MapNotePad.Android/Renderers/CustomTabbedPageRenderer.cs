using Android.Content;
using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using AndroidX.ViewPager.Widget;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.Internal;
using Google.Android.Material.Tabs;
using MapNotePad.Controls;
using MapNotePad.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace MapNotePad.Droid.Renderers
{
    public class CustomTabbedPageRenderer : TabbedPageRenderer
    {
        bool firstStartOnLayout = true;

        Xamarin.Forms.TabbedPage tabbedPage;

        BottomNavigationView bottomNavigationView;

        public CustomTabbedPageRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                tabbedPage = e.NewElement as CustomTabbedPage;
                bottomNavigationView = (GetChildAt(0) as Android.Widget.RelativeLayout).GetChildAt(1) as BottomNavigationView;
            }
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            if (bottomNavigationView != null)
            {
                var item = bottomNavigationView.Menu.GetItem(bottomNavigationView.SelectedItemId);

                SetupBottomNavigationView(item);
            }

            if (firstStartOnLayout)
            {
                var bottomNavMenuView = bottomNavigationView.GetChildAt(0) as BottomNavigationMenuView;

                for (int i = 0; i < bottomNavMenuView.ChildCount; i++)
                {
                    var item = bottomNavMenuView.GetChildAt(i) as BottomNavigationItemView;
                    var itemicon = item.GetChildAt(0);
                    var itemTitle = item.GetChildAt(1);

                    var IconImageView = (ImageView)itemicon;
                    var smallTextView = ((TextView)((BaselineLayout)itemTitle).GetChildAt(0));
                    var largeTextView = ((TextView)((BaselineLayout)itemTitle).GetChildAt(1));

                    IconImageView.TranslationX -= 110;
                    IconImageView.TranslationY += 20;

                    smallTextView.TextSize = 18;
                    smallTextView.TranslationY -= 10;
                    smallTextView.TranslationX += 25;
                    smallTextView.SetWidth(smallTextView.Width + 70);
                    smallTextView.SetHeight(smallTextView.Height + 50);

                    largeTextView.TextSize = 18;
                    largeTextView.TranslationY -= 10;
                    largeTextView.TranslationX += 25;
                    largeTextView.SetWidth(largeTextView.Width + 70);
                    largeTextView.SetHeight(largeTextView.Height + 50);
                }
            }

            firstStartOnLayout = false;
        }

        void SetupBottomNavigationView(IMenuItem item)
        {
            int itemHeight = bottomNavigationView.Height;
            int itemWidth = (bottomNavigationView.Width / Element.Children.Count);

            SolidColorBrush brush = Brush.Silver;
            brush.Color = new Xamarin.Forms.Color(0.945, 0.9529, 0.992);

            GradientDrawable button = new GradientDrawable();
            button.UpdateBackground(brush, itemHeight, itemWidth);
            button.SetShape(ShapeType.Rectangle);

            int leftOffset = item.ItemId * itemWidth;
            int rightOffset = itemWidth * (Element.Children.Count - (item.ItemId + 1));

            var layerDrawable = new LayerDrawable(new Drawable[] { button });
            layerDrawable.SetLayerInset(0, leftOffset, 0, rightOffset, 0);

            bottomNavigationView.SetBackground(layerDrawable);
        }
    }
}
