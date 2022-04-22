using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public class PublicTransport : IVehicle
    {
        public PublicTransportType Type { get; set; }

        public PublicTransport(PublicTransportType type)
        {
            Type = type;
        }
    }
}
