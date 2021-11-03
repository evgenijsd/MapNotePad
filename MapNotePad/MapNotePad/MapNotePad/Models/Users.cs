using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotePad.Models
{
    public class Users : IEntity
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateCreating { get; set; }
    }
}
