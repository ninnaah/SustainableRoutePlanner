using Models.Transport;
using System;

namespace Models
{
    public class RouteRequest
    {
        public RouteRequest(string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime, string routeType)
        {
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

            RouteType = routeType;
            
            Id = Guid.NewGuid();
        }

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        //fastest, shortest, bicycle 
        public string RouteType { get; set; }

        //meh?
        public Guid Id { get; set; }

    }
}
