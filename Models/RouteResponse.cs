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
            Maneuvers = new List<RouteManeuver>();
            RouteEmissions = new RouteEmissions();
        }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public double Duration { get; set; } //min

        public double Distance { get; set; }

        public List<RouteManeuver> Maneuvers { get; set; }

        public RouteEmissions RouteEmissions { get; set; }

        public Guid Id { get; set; }

    }
}
