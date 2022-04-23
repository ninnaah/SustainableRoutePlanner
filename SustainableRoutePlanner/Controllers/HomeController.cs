using EmissionCalculator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;
using Models.Transport;
using ServiceAgents;
using SustainableRoutePlanner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SustainableRoutePlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private HttpClient _client;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

            _client = new HttpClient();

        }

        public IActionResult Index()
        {
            tmpMethod();

            return View();
        }

        public async void tmpMethod()
        {
            CarEmissionCalculator calculator = new CarEmissionCalculator();
            calculator.CalcEmissions();
            /*Car car = new Car();
            RouteRequestModel model = new RouteRequestModel(car, "Gumpendorferstraße 103, Wien, Österreich, 1060", "Heiligenstädterstraße 33, Wien, Österreich, 1190", DateTime.Now, DateTime.Now, "bicycle");
            MapQuestAgent agent = new MapQuestAgent();
            agent.loadConfig();
            await agent.GetRouteValues(model);*/
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
