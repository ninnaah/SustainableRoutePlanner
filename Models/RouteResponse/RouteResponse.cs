using Models.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RouteResponse : RouteResponseBase
    {
        public RouteResponse(Guid id)
        {
            Id = id;
            Maneuvers = new List<RouteManeuver>();
            RouteEmissions = new RouteEmissions();
        }

        public List<RouteManeuver> Maneuvers { get; set; }

        public string TransportType { get; set; }

    }
}
