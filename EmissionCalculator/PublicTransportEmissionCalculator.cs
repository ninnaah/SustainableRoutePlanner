using Microsoft.Extensions.Configuration;
using Models;
using Models.Transport;
using ServiceAgents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public class PublicTransportEmissionCalculator : IEmissionCalculator
    {
        public EmissionFactorsLoader EmissionFactors { get; set; }
        public IPublicTransport PublicTransport;

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public PublicTransportEmissionCalculator(EmissionFactorsLoader emissionFactors, IPublicTransport publicTransport, string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime)
        {
            EmissionFactors = emissionFactors;
            PublicTransport = publicTransport;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

        }

        public async Task<RouteResponse> CalcEmissions()
        {
            RouteResponse response = await GetRoute();

            if (PublicTransport.GetType() == typeof(PublicTransport))
            {
                response.RouteEmissions.CO2Equivalent = EmissionFactors.EmissionFactors.PublicTransport.CO2Equivalent * response.Distance;
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.PublicTransport.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.PublicTransport.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.PublicTransport.PM10 * response.Distance;
            }
            else if (PublicTransport.GetType() == typeof(Bus))
            {
                response.RouteEmissions.CO2Equivalent = EmissionFactors.EmissionFactors.Bus.CO2Equivalent * response.Distance;
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.Bus.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.Bus.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.Bus.PM10 * response.Distance;
            }

            return response;
        }

        public async Task<RouteResponse> GetRoute()
        {
            throw new NotImplementedException();
        }


    }
}
