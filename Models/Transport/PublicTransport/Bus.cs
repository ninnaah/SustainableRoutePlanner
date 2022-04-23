using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public class Bus : IPublicTransport
    {
        public double CO2Equivalent { get; set; }
        public double CO2 { get; set; }
        public double NOX { get; set; }
        public double PM10 { get; set; }
    }
}
