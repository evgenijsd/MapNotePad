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
    }
}