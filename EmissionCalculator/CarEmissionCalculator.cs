using Microsoft.Extensions.Configuration;
using Models;
using Models.Transport;
using Newtonsoft.Json;
using ServiceAgents;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public class CarEmissionCalculator : IEmissionCalculator
    {
        public EmissionFactorsLoader EmissionFactors { get; set; }
        public ICar Car;

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public CarEmissionCalculator(EmissionFactorsLoader emissionFactors, ICar car, string departureLocation, string arrivalLocation, DateTime? departureTime, DateTime? arrivalTime)
        {
            EmissionFactors = emissionFactors;
            Car = car;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;

        }

        public async Task<RouteResponse> CalcEmissions()
        {
            RouteResponse response = await GetRoute();

            if (Car.GetType() == typeof(GasCar))
            {
                response.RouteEmissions.CO2Equivalent = EmissionFactors.EmissionFactors.GasCar.CO2Equivalent * response.Distance;
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.GasCar.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.GasCar.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.GasCar.PM10 * response.Distance;
            }
            else if (Car.GetType() == typeof(DieselCar))
            {
                response.RouteEmissions.CO2Equivalent = EmissionFactors.EmissionFactors.DieselCar.CO2Equivalent * response.Distance;
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.DieselCar.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.DieselCar.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.DieselCar.PM10 * response.Distance;
            }
            else if (Car.GetType() == typeof(ElectricCar))
            {
                response.RouteEmissions.CO2Equivalent = EmissionFactors.EmissionFactors.ElectricCar.CO2Equivalent * response.Distance;
                response.RouteEmissions.CO2 = EmissionFactors.EmissionFactors.ElectricCar.CO2 * response.Distance;
                response.RouteEmissions.NOX = EmissionFactors.EmissionFactors.ElectricCar.NOX * response.Distance;
                response.RouteEmissions.PM10 = EmissionFactors.EmissionFactors.ElectricCar.PM10 * response.Distance;
            }

            return response;
        }

        public async Task<RouteResponse> GetRoute()
        {
            RouteRequest reqModel = new RouteRequest(DepartureLocation, ArrivalLocation, DepartureTime, ArrivalTime, "fastest");
            MapQuestAgent agent = new MapQuestAgent();
            return await agent.GetRouteValues(reqModel);
        }

    }
}
