using Models;
using Models.Transport;
using ServiceAgents;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace EmissionCalculator
{
    public class BicycleEmissionCalculator : IEmissionCalculator
    {
        public EmissionFactorsLoader EmissionFactors { get; set; }
        public IBicycle Bicycle;

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public BicycleEmissionCalculator(EmissionFactorsLoader emissionFactors, IBicycle bicycle, string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime)
        {
            EmissionFactors = emissionFactors;
            Bicycle = bicycle;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

        }

        public async Task<RouteResponse> CalcEmissions()
        {
            RouteResponse response = await GetRoute();

            if(Bicycle.GetType() == typeof(Bicycle))
            {
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.Bicycle.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.Bicycle.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.Bicycle.PM10 * response.Distance;
            }
            else if(Bicycle.GetType() == typeof(EBike))
            {
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.EBike.CO2 * response.Distance;
            }

            return response;
        }

        public async Task<RouteResponse> GetRoute()
        {
            MapQuestRequest reqModel = new MapQuestRequest(DepartureLocation, ArrivalLocation, DepartureTime, ArrivalTime, "bicycle");
            MapQuestAgent agent = new MapQuestAgent();
            return await agent.GetRouteValues(reqModel);

        }

    }
}
