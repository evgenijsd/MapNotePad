using CoreGraphics;
using Foundation;
using MapNotePad.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;

namespace MapNotePad.iOS
{
    public class UITouchesGestureRecognizer : UIGestureRecognizer
    {
        #region -- Private Members --

        private ClickableContentView _element;
        private UIView _nativeView;

        #endregion

        public UITouchesGestureRecognizer(ClickableContentView element, UIView nativeView)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (nativeView == null)
            {
                throw new ArgumentNullException("nativeView");
            }

            _element = element;
            _nativeView = nativeView;
        }

        private IEnumerable<NGraphics.Point> GetTouchPoints(Foundation.NSSet touches)
        {
            var points = new List<NGraphics.Point>((int)touches.Count);

            foreach (UITouch touch in touches)
            {
                CGPoint touchPoint = touch.LocationInView(_nativeView);
                points.Add(new NGraphics.Point((double)touchPoint.X, (double)touchPoint.Y));
            }

            return points;
        }

        public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesBegan(touches, evt);

            if (this._element.TouchesBegan(GetTouchPoints(touches)))
            {
                this.State = UIGestureRecognizerState.Began;
            }
            else
            {
                this.State = UIGestureRecognizerState.Cancelled;
            }
        }

        public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            if (this._element.TouchesMoved(GetTouchPoints(touches)))
            {
                this.State = UIGestureRecognizerState.Changed;
            }
        }

        public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            if (this._element.TouchesEnded(GetTouchPoints(touches)))
            {
                this.State = UIGestureRecognizerState.Ended;
            }
        }

        public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
        {
            base.TouchesCancelled(touches, evt);

            this._element.TouchesCancelled(GetTouchPoints(touches));

            this.State = UIGestureRecognizerState.Cancelled;
        }
    }
}