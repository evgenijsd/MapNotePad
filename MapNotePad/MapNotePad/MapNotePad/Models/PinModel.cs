using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Models
{
    public class PinModel : IEntity
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public Pin Pin { get; set; }
        public int User { get; set; }
        public bool Favorites { get; set; }
    }
}
