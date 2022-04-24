using Models;
using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableRoutePlanner.Models
{
    public class Route
    {
        public Route(string departureAdress, string arrivalAdress, DateTime? departureTime, DateTime? arrivalTime)
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

        public RouteResponse PublicTransportRoute { get; set; }
        public RouteResponse CarRoute { get; set; }
        public RouteResponse BicycleRoute { get; set; }
    }
}