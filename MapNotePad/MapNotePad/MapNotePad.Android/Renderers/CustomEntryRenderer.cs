using AgentLocator.Droid.Renderers;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Graphics.Drawables.Shapes;
using Android.Widget;
using MapNotePad.Controls;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace AgentLocator.Droid.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        private Thickness TextPadding = new Thickness(0);
        public CustomEntryRenderer(Context context) : base(context) { }
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                int r = 10;
                var corners = new float[] { r, r, r, r, r, r, r, r };

                Control.Background = new ColorDrawable(Android.Graphics.Color.White);

                if (Control is EditText nativeEditText)
                {
                    var textPadding = this.TextPadding;
                    var shape = new ShapeDrawable(new RoundRectShape(corners, null, null));
                    shape.Paint.Color = Xamarin.Forms.Color.Black.ToAndroid();
                    shape.Paint.SetStyle(Paint.Style.Stroke);

                    nativeEditText.Background = shape;
                    nativeEditText.SetPadding(40, 10, 10, 10);
                }
            }
        }
    }
}
