using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public interface ICar
    {
        double CO2Equivalent { get; set; }
        double CO2 { get; set; }
        double NOX { get; set; }
        double PM10 { get; set; }
    }
}
