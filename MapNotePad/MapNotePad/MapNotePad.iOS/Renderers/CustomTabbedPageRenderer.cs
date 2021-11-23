using Foundation;
using MapNotePad.Controls;
using MapNotePad.iOS.Renderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomTabbedPage), typeof(CustomTabbedPageRenderer))]
namespace MapNotePad.iOS.Renderers
{
    public class CustomTabbedPageRenderer : TabbedRenderer
    {
        public CustomTabbedPageRenderer()
        {
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();

            if (Element is TabbedPage)
                if (TabBar?.Items != null)
                {

                    foreach (var item in TabBar.Items)
                    {
                        item.ImageInsets = new UIEdgeInsets(top: 15, left: -30, bottom: -20, right: 25);
                        item.TitlePositionAdjustment = new UIOffset(0, -14);
                        var txtFont = new UITextAttributes() { Font = UIFont.SystemFontOfSize(17) };
                        item.SetTitleTextAttributes(txtFont, UIControlState.Normal);
                    }
                }
        }

    }
}
