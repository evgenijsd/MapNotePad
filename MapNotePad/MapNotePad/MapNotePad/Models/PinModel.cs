using SQLite;
using System;

namespace MapNotePad.Models
{
    public class PinModel : IEntity
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int User { get; set; }
        public bool Favourite { get; set; }
        public DateTime Date { get; set; }
    }
}
