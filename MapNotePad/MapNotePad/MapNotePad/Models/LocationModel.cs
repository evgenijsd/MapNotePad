using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Models
{
    public class LocationModel
    {
        public LocationModel()
        { }

        public LocationModel(string description, string address, Position position)
        {
            Description = description;
            Address = address;
            Position = position;
        }

        public Position Position
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public string Address
        {
            get; set;
        }
    }
}
