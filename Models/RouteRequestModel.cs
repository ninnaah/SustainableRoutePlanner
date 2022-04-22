using System;

namespace Models
{
    public class RouteRequestModel
    {
        public RouteRequestModel(string departureLocation, string arrivalLocation, DateTime departureTime, DateTime arrivalTime, string transportMode)
        {
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

            TransportMode = transportMode;

            Id = Guid.NewGuid();
        }
        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        //as enum
        public string TransportMode { get; set; }

        //meh?
        public Guid Id { get; set; }

    }
}
