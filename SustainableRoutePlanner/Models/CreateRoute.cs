using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SustainableRoutePlanner.Models
{
    public class CreateRoute
    {
        public CreateRoute()
        {

        }

        public CreateRoute(string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime)
        {
            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

    }
}