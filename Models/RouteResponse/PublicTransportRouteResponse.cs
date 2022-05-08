using Models.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PublicTransportRouteResponse : RouteResponseBase
    {
        public PublicTransportRouteResponse(Guid id)
        {
            Id = id;
            Maneuvers = new List<PublicTransportRouteManeuver>();
            RouteEmissions = new RouteEmissions();
        }

        public List<PublicTransportRouteManeuver> Maneuvers { get; set; }

    }
}
