using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace MapNotePad.Models
{
    public class CustomPin
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public Pin Pin { get; set; }
        public int User { get; set; }
    }
}
