using Microsoft.Extensions.Configuration;
using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
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
            loadConfig();
        }

        public void loadConfig()
        {
            IConfiguration configBuilder = new ConfigurationBuilder()
                .AddJsonFile("config.json", true)
                .Build();

            ConfigModel config = configBuilder.Get<ConfigModel>();
            _key = config.MapQuestKey;
            _dirPath = config.MapsPath;

            Debug.WriteLine($"Loaded configuration");
        }

        public async Task<string> GetRouteValues(RouteRequestModel routeReq)
        {
            Task<string> responseBodyTask = SendRouteRequest(routeReq);
            string responseBody = await responseBodyTask;

            return responseBody;
        }

        public static async Task<string> SendRouteRequest(RouteRequestModel routeReq)
        {
            int timeType = 0;

            //MM/DD/YYYY
            string date = "";

            //hh:mm
            string localTime = "";


            if (routeReq.ArrivalTime != null)
            {
                timeType = 3;
                date = routeReq.ArrivalTime?.ToString("MM/dd/yyyy");
                localTime = routeReq.ArrivalTime?.ToString("hh:mm");

            }
            else if (routeReq.DepartureTime != null)
            {
                timeType = 2;
                date = routeReq.DepartureTime?.ToString("MM/dd/yyyy");
                localTime = routeReq.DepartureTime?.ToString("hh:mm");
            }

            string getRequest = $"http://www.mapquestapi.com/directions/v2/route?key={_key}&from={routeReq.ArrivalLocation}&to={routeReq.DepartureLocation}&routeType={routeReq.TransportMode}&timeType={timeType}&date={date}&localTime={localTime}";
            Debug.WriteLine($"Directions Request: {getRequest}");


            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);

            _boundingBox = obj["route"]["boundingBox"] as JObject;
            string sessionId = (string)obj["route"]["sessionId"];

            Debug.WriteLine("Got response from directions API");

            SendMapRequest(routeReq, sessionId);

            return responseBody;
        }

        public static async void SendMapRequest(RouteRequestModel routeReq, string sessionId)
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
