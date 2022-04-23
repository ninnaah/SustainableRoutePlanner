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
    public class CarEmissionCalculator : IEmissionCalculator
    {
        public ICar Car;

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }

        public CarEmissionCalculator(ICar car, string departureLocation, string arrivalLocation, DateTime departureTime, DateTime arrivalTime)
        {
            Car = car;

            DepartureLocation = departureLocation;
            ArrivalLocation = arrivalLocation;

            DepartureTime = departureTime;
            ArrivalTime = arrivalTime;
        }

        public void LoadEmissionFactors()
        {
            IConfiguration configBuilder = new ConfigurationBuilder()
                .AddJsonFile("emissionFactors.json", true)
                .Build();

            EmissionFactorsConfig config = configBuilder.Get<EmissionFactorsConfig>();

            Debug.WriteLine($"Loaded emissionFactors");
        }

        public async void CalcEmissions()
        {

            //await GetRoute();

            //cald emissionfac * km

        }

        public async Task<RouteResponse> GetRoute()
        {
            RouteRequest reqModel = new RouteRequest("Gumpendorferstraße 103, Wien, Österreich, 1060", "Heiligenstädterstraße 33, Wien, Österreich, 1190", DateTime.Now, DateTime.Now, "fastest");
            MapQuestAgent agent = new MapQuestAgent();
            return await agent.GetRouteValues(reqModel);
        }

    }
}
