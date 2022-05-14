using EmissionCalculator;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Transport;
using SustainableRoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SustainableRoutePlanner.Controllers
{
    public class RouteController : Controller
    {
        public EmissionFactorsLoader EmissionLoader;

        public RouteController()
        {
            EmissionLoader = new EmissionFactorsLoader();
            EmissionLoader.LoadEmissionFactors();
        }

        //Get Route/Create
        public IActionResult Create()
        {
            return View();
        }

        //Post Route/Display
        [HttpPost]
        public async Task<IActionResult> Display(CreateRoute createRoute)
        {
            ICar car = null;
            string carType = "";

            if (createRoute.CarType == "diesel")
            {
                car = new DieselCar();
                carType = "Diesel";
            }
            else if (createRoute.CarType == "gas")
            {
                car = new GasCar();
                carType = "Benzin";
            }
            else if (createRoute.CarType == "electric")
            {
                car = new ElectricCar();
                carType = "Elektrisch";
            }


            IBicycle bicycle = null;
            string bicycleType = "";

            if (createRoute.BicycleType == "bicycle")
            {
                bicycle = new Bicycle();
                bicycleType = "Fahrrad"; 
            }
            else if (createRoute.BicycleType == "electric")
            {
                bicycle = new EBike();
                bicycleType = "E-Bike";
            }

            CarEmissionCalculator carCalculator = new CarEmissionCalculator(EmissionLoader, car, createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            RouteResponse carResponse = await carCalculator.CalcEmissions();
            carResponse.TransportType = carType;

            PublicTransportEmissionCalculator publicTransportCalculator = new PublicTransportEmissionCalculator(EmissionLoader, createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            PublicTransportRouteResponse publicTransportResponse = await publicTransportCalculator.CalcEmissions();

            BicycleEmissionCalculator bicycleCalculator = new BicycleEmissionCalculator(EmissionLoader, bicycle, createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            RouteResponse bicycleResponse = await bicycleCalculator.CalcEmissions();
            bicycleResponse.TransportType = bicycleType;

            Route route = new Route(createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);

            route.CarRoute = carResponse;
            route.BicycleRoute = bicycleResponse;
            route.PublicTransportRoute = publicTransportResponse;

            return View(route);
        }



    }
}