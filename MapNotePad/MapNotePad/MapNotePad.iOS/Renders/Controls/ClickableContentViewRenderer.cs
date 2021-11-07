using AgentLocator.iOS.Renderers.Controls;
using MapNotePad.Controls;
using MapNotePad.iOS;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ClickableContentView), typeof(ClickableContentViewRenderer))]
namespace AgentLocator.iOS.Renderers.Controls
{
    [Preserve(AllMembers = true)]
    public class ClickableContentViewRenderer : VisualElementRenderer<ClickableContentView>
    {
        /// <summary>
        /// The gesture recognizer.
        /// </summary>
        private UITouchesGestureRecognizer _gestureRecognizer;

        /// <summary>
        /// Used for registration with dependency service
        /// </summary>
        public new static void Init()
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
                if (null != _gestureRecognizer)
                {
                    RemoveGestureRecognizer(_gestureRecognizer);
                    _gestureRecognizer = null;
                }

                e.OldElement.OnInvalidate -= HandleInvalidate;
            }

            if (e.NewElement != null)
            {
                e.NewElement.OnInvalidate += HandleInvalidate;

                if ((null == _gestureRecognizer) && (null != NativeView))
                {
                    _gestureRecognizer = new UITouchesGestureRecognizer(e.NewElement, NativeView);
                    NativeView.AddGestureRecognizer(_gestureRecognizer);
                }

                e.NewElement.Invalidate();
            }
        }

        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == ClickableContentView.IsClippedToBoundsProperty.PropertyName)
                Layer.MasksToBounds = Element.IsClippedToBounds;
            else if (e.PropertyName == ClickableContentView.BackgroundColorProperty.PropertyName)
                Element.Invalidate();
            else if (e.PropertyName == ClickableContentView.IsVisibleProperty.PropertyName)
                Element.Invalidate();
        }


        #region Touch Handlers

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            // Ignore buggy Xamarin touch events on iOS
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
            SetNeedsDisplay();
        }
        #endregion
    }
}