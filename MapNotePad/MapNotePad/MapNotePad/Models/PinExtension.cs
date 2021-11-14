using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotePad.Models
{
    public static class PinExtension
    {
        public static PinModel ToPinModel(this PinView pin)
        {
            PinModel n = new PinModel()
            {
                Id = pin.Id,
                Name = pin.Name,
                Description = pin.Description,
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                User = pin.User,
                Favourite = pin.Favourite,
                Date = pin.Date
            };
            return n;
        }

        public static PinView ToPinView(this PinModel pin)
        {
            PinView n = new PinView()
            {
                Id = pin.Id,
                Name = pin.Name,
                Description = pin.Description,
                Latitude = pin.Latitude,
                Longitude = pin.Longitude,
                User = pin.User,
                Favourite = pin.Favourite,
                Date = pin.Date,
            };
            if (n.Favourite) n.Image = "";
            return n;
        }
    }
}
