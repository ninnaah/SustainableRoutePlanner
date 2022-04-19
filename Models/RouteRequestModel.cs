using System;

namespace Models
{
    public class RouteRequestModel
    {
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public DateTime TransportMode { get; set; }

    }
}
