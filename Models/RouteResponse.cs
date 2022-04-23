using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RouteResponse
    {
        public RouteResponse()
        {
            Maneuvers = new List<RouteResponseManeuver>();
        }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public double Duration { get; set; } //min

        public double Distance { get; set; }

        public List<RouteResponseManeuver> Maneuvers { get; set; }
    }
}
