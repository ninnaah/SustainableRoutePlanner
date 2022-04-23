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

        //Post Route/Create
        [HttpPost]
        public async Task<IActionResult> Display(CreateRoute createRoute)
        {
            CarEmissionCalculator carCalculator = new CarEmissionCalculator(EmissionLoader, new GasCar(), "Gumpendorferstraße 103, Wien, Österreich, 1060", "Heiligenstädterstraße 33, Wien, Österreich, 1190", new DateTime(2022, 4, 24, 5, 10, 20), null);
            RouteResponse carResponse = await carCalculator.CalcEmissions();
            Debug.WriteLine(carResponse);


            /*BicycleEmissionCalculator bicycleCalculator = new BicycleEmissionCalculator(EmissionLoader, new EBike(), "Gumpendorferstraße 103, Wien, Österreich, 1060", "Heiligenstädterstraße 33, Wien, Österreich, 1190", null, new DateTime(2022, 4, 24, 5, 10, 20));
            RouteResponse bicycleResponse = await bicycleCalculator.CalcEmissions();
            Debug.WriteLine(bicycleResponse);*/

            Route route = new Route();
            route.CarRoute = carResponse;

            return View(route);
        }



    }
}