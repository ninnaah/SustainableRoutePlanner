using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RouteResponseModel
    {
        public RouteResponseModel(IVehicle vehicle, DateTime departureTime, DateTime arrivalTime, double duration, double distance)
        {
            Vehicle = vehicle;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
            Duration = duration;

            Distance = distance;
        }

        public IVehicle Vehicle { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }
        public double Duration { get; set; }

        public double Distance { get; set; }
    }
}
