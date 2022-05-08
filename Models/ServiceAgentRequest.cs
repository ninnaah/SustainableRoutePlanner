using Models.Transport;
using System;

namespace Models
{
    public class ServiceAgentRequest
    {
        public ServiceAgentRequest(string departureAdress, string arrivalAdress, DateTime? departureTime, DateTime? arrivalTime, string routeType)
        {
            DepartureAdress = departureAdress;
            ArrivalAdress = arrivalAdress;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

            RouteType = routeType;
            
            Id = Guid.NewGuid();
        }

        public ServiceAgentRequest(string departureAdress, string arrivalAdress, DateTime? departureTime, DateTime? arrivalTime)
        {
            DepartureAdress = departureAdress;
            ArrivalAdress = arrivalAdress;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

            Id = Guid.NewGuid();
        }

        public string DepartureAdress { get; set; }
        public string ArrivalAdress { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        //fastest, shortest, bicycle 
        public string RouteType { get; set; }

        //meh?
        public Guid Id { get; set; }

    }
}
