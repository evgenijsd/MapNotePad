
using System.IO;
using System.Reflection;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace MapNotePad.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : BaseContentPage
    {
        public MapPage()
        {
            InitializeComponent();
            
        }

        void AddMapStyle()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MapNotePad.Resources.MapLightStyle.json";

            string styleFile;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName)) 
            using (StreamReader reader = new(stream))
            {
                styleFile = reader.ReadToEnd();
            }

            myMap.MapStyle = MapStyle.FromJson(styleFile);
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            AddMapStyle();
        }
    }
}