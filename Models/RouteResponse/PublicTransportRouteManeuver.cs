using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PublicTransportRouteManeuver
    {
        public string TransportName { get; set; }
        public string TransportType { get; set; }
        public List<string> Stops { get; set; }
        public double Distance { get; set; }
        public double Duration { get; set; }


        public string From { get; set; }
        public string To { get; set; }

        public PublicTransportRouteManeuver(string transportName, string transportType, double distance, double duration)
        {
            TransportName = transportName;
            TransportType = transportType;
            Stops = new List<string>();
            Distance = distance;
            Duration = duration;
        }

        //fußweg
        public PublicTransportRouteManeuver(string transport, string from, string to, double distance, double duration)
        {
            TransportType = transport;
            TransportName = transport;
            Duration = duration;
            Distance = distance;
            Stops = new List<string>();

            From = from;
            To = to;
        }


    }
}
