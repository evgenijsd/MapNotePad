using MapNotePad.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Controls
{
    public class MapView : Xamarin.Forms.GoogleMaps.Map
    {

        public Xamarin.Forms.GoogleMaps.Map Map { get; private set; }
        public MapView(List<CustomPin> customPins)
        {
            CustomPins = customPins;
        }

        public List<CustomPin> CustomPins { get; set; }
    }
}
