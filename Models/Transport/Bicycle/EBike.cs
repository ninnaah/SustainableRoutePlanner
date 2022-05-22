﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public class EBike : IBicycle
    {
        public double CO2 { get; set; }
        public double NOX { get; set; }
        public double PM10 { get; set; }
    }
}
