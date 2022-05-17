using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceAgents
{
    public class MapQuestAgent : IServiceAgent
    {
        private static JObject _boundingBox;

        private static string _key;
        private static string _dirPath;

        public MapQuestAgent()
        {
            LoadConfig();
        }

        private void LoadConfig()
        {
            IConfiguration configBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json", true)
                .Build();

            Config config = configBuilder.Get<Config>();
            _key = config.MapQuestKey;
            _dirPath = config.MapsPath;

            Debug.WriteLine($"Loaded configuration");
        }

        public async Task<IRouteResponse> GetRouteValues(ServiceAgentRequest routeReq)
        {
            routeReq = DetermineLocation(routeReq);

            Task<IRouteResponse> responseModelTask = SendRouteRequest(routeReq);
            IRouteResponse responseModel = await responseModelTask;

            return responseModel;
        }

        private ServiceAgentRequest DetermineLocation(ServiceAgentRequest routeReq)
        {
            routeReq.DepartureAdress = $"{routeReq.DepartureAdress}, Wien, Österreich";
            routeReq.ArrivalAdress = $"{routeReq.ArrivalAdress}, Wien, Österreich";

            return routeReq;
        }

        private static async Task<IRouteResponse> SendRouteRequest(ServiceAgentRequest routeRequest)
        {
            RouteResponse routeResponse = new RouteResponse(routeRequest.Id);

            int timeType = 0;
            string date = "";
            string localTime = "";

            if (routeRequest.ArrivalTime == null && routeRequest.DepartureTime == null)
            {
                routeResponse.DepartureTime = DateTime.Now;
            }
            else if (routeRequest.ArrivalTime != null)
            {
                timeType = 3;
                date = routeRequest.ArrivalTime?.ToString("MM/dd/yyyy");
                localTime = routeRequest.ArrivalTime?.ToString("hh:mm");
                routeResponse.ArrivalTime = routeRequest.ArrivalTime;

            }
            else if (routeRequest.DepartureTime != null)
            {
                timeType = 2;
                date = routeRequest.DepartureTime?.ToString("MM/dd/yyyy");
                localTime = routeRequest.DepartureTime?.ToString("hh:mm");
                routeResponse.DepartureTime = routeRequest.DepartureTime;
            }

            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&unit=k&to={routeRequest.ArrivalAdress}&from={routeRequest.DepartureAdress}&routeType={routeRequest.RouteType}&timeType={timeType}&date={date}&localTime={localTime}";
            Debug.WriteLine($"Directions Request: {getRequest}");

            
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.WriteLine("Got response from directions API");

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            _boundingBox = obj["route"]["boundingBox"] as JObject;
            string sessionId = (string)obj["route"]["sessionId"];

            SendMapRequest(routeRequest, sessionId);


            routeResponse.Duration = Math.Round((double)obj["route"]["time"] / 60, 2);
            routeResponse.Distance = (double) obj["route"]["distance"];

            //maneuvers
            JObject legs = obj["route"]["legs"][0] as JObject;

            List<JObject> maneuvers = new List<JObject>();
            foreach (JObject maneuver in legs["maneuvers"])
            {
                maneuvers.Add(maneuver);
            }

            foreach (JObject maneuver in maneuvers)
            {
                RouteManeuver direction = new RouteManeuver((string)maneuver["narrative"], (string)maneuver["iconUrl"]);
                routeResponse.Maneuvers.Add(direction);
            }

            return routeResponse;
        }

        private static async void SendMapRequest(ServiceAgentRequest routeReq, string sessionId)
        {
            try
            {
                if (_boundingBox == null)
                {
                    return;
                }

                string lowerRightLng = (string)_boundingBox["lr"]["lng"];
                string lowerRightLat = (string)_boundingBox["lr"]["lat"];
                string upperLeftLng = (string)_boundingBox["ul"]["lng"];
                string upperLeftLat = (string)_boundingBox["ul"]["lat"];

                string filePath = $@"{_dirPath}/{routeReq.Id}.png";

                string getRequest = $"https://www.mapquestapi.com/staticmap/v5/map?key={_key}&size=1240,960&session={sessionId}&boundingBox={upperLeftLat},{upperLeftLng},{lowerRightLat},{lowerRightLng}&zoom=15";
                Debug.WriteLine($"StaticMap Request: {getRequest}");

                using (var client = new WebClient())
                using (var completedSignal = new AutoResetEvent(false))
                {
                    client.DownloadFileCompleted += (s, e) =>
                    {
                        Debug.WriteLine("Download file completed.");
                        completedSignal.Set();
                    };

                    client.DownloadFileAsync(new Uri(getRequest), filePath);

                    completedSignal.WaitOne();
                }

                Debug.WriteLine("Got response from staticMap API");
            }
            catch (Exception ex)
            {
                File.Delete(@$"{_dirPath}/{routeReq.Id}.png");
                Debug.WriteLine($"Cannot load tourmap: {ex.Message}");
            }

            Debug.WriteLine("Downloaded map");

        }
    }
}
