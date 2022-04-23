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
using System.Threading.Tasks;

namespace ServiceAgents
{
    public class MapQuestAgent
    {
        private static JObject _boundingBox;

        private static string _key;
        private static string _dirPath;

        public MapQuestAgent()
        {
            LoadConfig();
        }

        public void LoadConfig()
        {
            IConfiguration configBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json", true)
                .Build();

            Config config = configBuilder.Get<Config>();
            _key = config.MapQuestKey;
            _dirPath = config.MapsPath;

            Debug.WriteLine($"Loaded configuration");
        }

        public async Task<RouteResponse> GetRouteValues(RouteRequest routeReq)
        {
            Task<RouteResponse> responseModelTask = SendRouteRequest(routeReq);
            RouteResponse responseModel = await responseModelTask;

            return responseModel;
        }

        public static async Task<RouteResponse> SendRouteRequest(RouteRequest routeRequest)
        {
            RouteResponse routeResponse = new RouteResponse();
            routeResponse.Id = routeRequest.Id;

            int timeType = 0;
            string date = "";
            string localTime = "";

            if (routeRequest.ArrivalTime != null)
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

            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={routeRequest.ArrivalLocation}&to={routeRequest.DepartureLocation}&routeType={routeRequest.RouteType}&timeType={timeType}&date={date}&localTime={localTime}";
            Debug.WriteLine($"Directions Request: {getRequest}");


            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            _boundingBox = obj["route"]["boundingBox"] as JObject;
            string sessionId = (string)obj["route"]["sessionId"];

            Debug.WriteLine("Got response from directions API");

            SendMapRequest(routeRequest, sessionId);


            routeResponse.Duration = (double) obj["route"]["time"]/60;
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

        public static async void SendMapRequest(RouteRequest routeReq, string sessionId)
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


                using WebClient client = new();
                await client.DownloadFileTaskAsync(new Uri(getRequest), filePath);
                Debug.WriteLine("Got response from staticMap API");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                File.Delete(@$"{_dirPath}/{routeReq.Id}.png");
                Debug.WriteLine($"Cannot load tourmap: {ex.Message}");
            }

            Debug.WriteLine("Downloaded map");

        }
    }
}
