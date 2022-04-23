using Models;
using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SustainableRoutePlanner.Models
{
    public class Route
    {
        public RouteResponse PublicTransportRoute { get; set; }
        public RouteResponse CarRoute { get; set; }
        public RouteResponse BicycleRoute { get; set; }
    }
}