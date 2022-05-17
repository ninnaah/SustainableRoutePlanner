using Models.Transport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RouteResponse : IRouteResponse
    {
        public RouteResponse(Guid id)
        {
            Id = id;
            Maneuvers = new List<RouteManeuver>();
            RouteEmissions = new RouteEmissions();
        }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public double Duration { get; set; } //min
        public double Distance { get; set; } //km

        public RouteEmissions RouteEmissions { get; set; }
        public List<RouteManeuver> Maneuvers { get; set; }

        public Guid Id { get; set; }

        public string TransportType { get; set; }

    }
}
