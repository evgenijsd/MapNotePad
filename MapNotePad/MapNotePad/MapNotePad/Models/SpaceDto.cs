﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MapNotePad.Models
{
    public class SpaceDto
    {
        public string Title { get; set; }
        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
}
