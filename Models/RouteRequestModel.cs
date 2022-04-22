using Models.Transport;
using System;

namespace Models
{
    public class RouteRequestModel
    {
        public RouteRequestModel(IVehicle vehicle, string departureLocation, string arrivalLocation, DateTime departureTime, DateTime arrivalTime, string routeType)
        {
            Vehicle = vehicle;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

            if(Vehicle.GetType() == typeof(Bike) || Vehicle.GetType() == typeof(EBike))
            {
                RouteType = "bicycle";
            }
            else
            {
                RouteType = routeType;
            }

            Id = Guid.NewGuid();
        }

        public IVehicle Vehicle { get; set; }

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public string RouteType { get; set; }

        //meh?
        public Guid Id { get; set; }

    }
}
