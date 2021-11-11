
using Android.Renderers.Controls;
using Android.Runtime;
using Android.Views;
using Controls;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ClickableContentView), typeof(ClickableContentViewRenderer))]
namespace MapNotePad.Droid.Renderers.Controls
{
    [Preserve(AllMembers = true)]
    public class ClickableContentViewRenderer : VisualElementRenderer<ClickableContentView>
    {
        public ClickableContentViewRenderer(Android.Content.Context context) : base(context)
        {
        }

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public static void Init()
        {
            var temp = DateTime.Now;
        }

        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<ClickableContentView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.OnInvalidate -= HandleInvalidate;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;
            }

            // Lets have a clear background
            this.SetBackgroundColor(Android.Graphics.Color.Transparent);

            Invalidate();
        }

        /// <summary>
        /// Override to avoid setting the background to a given color
        /// </summary>
        protected override void UpdateBackgroundColor()
        {
            // Do NOT call update background here.
            // base.UpdateBackgroundColor();
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ClickableContentView.BackgroundColorProperty.PropertyName)
                Element.Invalidate();
            else if (e.PropertyName == ClickableContentView.IsVisibleProperty.PropertyName)
                Element.Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Element != null)
            {
                Element.OnInvalidate -= HandleInvalidate;
            }
            base.Dispose(disposing);
        }

        #region Native Drawing 

        #endregion

        #region Touch Handling

        /// <Docs>The motion event.</Docs>
        /// <returns>To be added.</returns>
        /// <para tool="javadoc-to-mdoc">Implement this method to handle touch screen motion events.</para>
        /// <format type="text/html">[Android Documentation]</format>
        /// <since version="Added in API level 1"></since>
        /// <summary>
        /// Raises the touch event event.
        /// </summary>
        /// <param name="e">E.</param>
        public override bool OnTouchEvent(MotionEvent e)
        {
            var scale = Element.Width / Width;

            var touchInfo = new List<NGraphics.Point>();
            for (var i = 0; i < e.PointerCount; i++)
            {
                var coord = new MotionEvent.PointerCoords();
                e.GetPointerCoords(i, coord);
                touchInfo.Add(new NGraphics.Point(coord.X * scale, coord.Y * scale));
            }

            var result = false;

            // Handle touch actions
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    if (Element != null)
                        result = Element.TouchesBegan(touchInfo);
                    break;

                case MotionEventActions.Move:
                    if (Element != null)
                        result = Element.TouchesMoved(touchInfo);
                    break;

                case MotionEventActions.Up:
                    if (Element != null)
                        result = Element.TouchesEnded(touchInfo);
                    break;

                case MotionEventActions.Cancel:
                    if (Element != null)
                        result = Element.TouchesCancelled(touchInfo);
                    break;
            }

            return result;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Handles the invalidate.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        private void HandleInvalidate(object sender, System.EventArgs args)
        {
            Invalidate();
        }

        /// <summary>
        /// Gets the size of the screen.
        /// </summary>
        /// <returns>The screen size.</returns>
        [Obsolete]
        protected Xamarin.Forms.Size GetScreenSize()
        {
            var metrics = Forms.Context.Resources.DisplayMetrics;
            return new Xamarin.Forms.Size(metrics.WidthPixels, metrics.HeightPixels);
        }
        #endregion
    }
}