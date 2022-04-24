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

        public CreateRoute(string departureAdress, string arrivalAdress, DateTime? departureTime, DateTime? arrivalTime)
        {
            DepartureAdress = departureAdress;
            ArrivalAdress = arrivalAdress;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public string DepartureAdress { get; set; }
        public string ArrivalAdress { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

    }
}