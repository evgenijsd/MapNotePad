using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps.Clustering;
using Xamarin.Forms.GoogleMaps.Clustering.iOS;

[assembly: ExportRenderer(typeof(ClusteredMap), typeof(ClusteredMapRenderer))]
namespace MapNotePad.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        [Obsolete]
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, "AppDelegate");
        }
    }
}
