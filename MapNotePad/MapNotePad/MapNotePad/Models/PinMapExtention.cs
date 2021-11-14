using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Models
{
    public static class PinMapExtention
    {
        public static Pin ToPin(this PinView pin)
        {
            Pin n = new Pin()
            {
                Type = PinType.Place,
                Label = pin.Name,
                Address = pin.Description,
                Position = new Position(pin.Latitude, pin.Longitude)
            };
            return n;
        }
        public static Pin ToPin(this PinModel pin)
        {
            Pin n = new Pin()
            {
                Type = PinType.Place,
                Label = pin.Name,
                Address = pin.Description,
                Position = new Position(pin.Latitude, pin.Longitude)
            };
            return n;
        }

    }
}
