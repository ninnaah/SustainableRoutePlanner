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
    public class PublicTransportEmissionCalculator : EmissionCalculator
    {

        public PublicTransportEmissionCalculator(EmissionFactorsLoader emissionFactors, string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime)
        {
            EmissionFactors = emissionFactors;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

        }

        public async Task<PublicTransportRouteResponse> CalcEmissions()
        {
            PublicTransportRouteResponse response = await GetRoute();

            if (response.DepartureTime != null)
            {
                response.ArrivalTime = response.DepartureTime?.AddMinutes(response.Duration);
            }
            else if (response.ArrivalTime != null)
            {
                response.DepartureTime = response.ArrivalTime?.AddMinutes(-response.Duration);
            }


            foreach (PublicTransportRouteManeuver maneuver in response.Maneuvers)
            {
                if (maneuver.TransportType == "Zug" || maneuver.TransportType == "S-Bahn" || maneuver.TransportType == "U-Bahn" 
                    || maneuver.TransportType == "Stadtbahn" || maneuver.TransportType == "Straßen-/Trambahn" || maneuver.TransportType == "Seil-/Zahnradbahn")
                {
                    response.RouteEmissions.CO2Equivalent += EmissionFactors.EmissionFactors.PublicTransport.CO2Equivalent * maneuver.Distance;
                    response.RouteEmissions.CO2 += EmissionFactors.EmissionFactors.PublicTransport.CO2 * maneuver.Distance;
                    response.RouteEmissions.NOX += EmissionFactors.EmissionFactors.PublicTransport.NOX * maneuver.Distance;
                    response.RouteEmissions.PM10 += EmissionFactors.EmissionFactors.PublicTransport.PM10 * maneuver.Distance;
                }
                else if (maneuver.TransportType == "Stadtbus" || maneuver.TransportType == "Regionalbus" || maneuver.TransportType == "Schnellbus")
                {
                    response.RouteEmissions.CO2Equivalent += EmissionFactors.EmissionFactors.Bus.CO2Equivalent * maneuver.Distance;
                    response.RouteEmissions.CO2 += EmissionFactors.EmissionFactors.Bus.CO2 * maneuver.Distance;
                    response.RouteEmissions.NOX += EmissionFactors.EmissionFactors.Bus.NOX * maneuver.Distance;
                    response.RouteEmissions.PM10 += EmissionFactors.EmissionFactors.Bus.PM10 * maneuver.Distance;
                }
            }


            return response;
        }

        public async Task<PublicTransportRouteResponse> GetRoute()
        {
            ServiceAgentRequest reqModel = new ServiceAgentRequest(DepartureLocation, ArrivalLocation, DepartureTime, ArrivalTime);
            EFAAgent agent = new EFAAgent();
            return await agent.GetRouteValues(reqModel);
        }


    }
}
