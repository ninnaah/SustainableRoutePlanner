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

            CarEmissionCalculator carCalculator = new CarEmissionCalculator(EmissionLoader, new GasCar(), createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            RouteResponse carResponse = await carCalculator.CalcEmissions();
            Debug.WriteLine(carResponse);

            PublicTransportEmissionCalculator publicTransportCalculator = new PublicTransportEmissionCalculator(EmissionLoader, new Bus(), createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            PublicTransportRouteResponse publicTransportResponse = await publicTransportCalculator.CalcEmissions();
            Debug.WriteLine(publicTransportResponse);

            BicycleEmissionCalculator bicycleCalculator = new BicycleEmissionCalculator(EmissionLoader, new EBike(), createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);
            RouteResponse bicycleResponse = await bicycleCalculator.CalcEmissions();
            Debug.WriteLine(bicycleResponse);

            Route route = new Route(createRoute.DepartureAdress, createRoute.ArrivalAdress, createRoute.DepartureTime, createRoute.ArrivalTime);

            route.CarRoute = carResponse;
            route.BicycleRoute = bicycleResponse;
            route.PublicTransportRoute = publicTransportResponse;

            return View(route);
        }



    }
}