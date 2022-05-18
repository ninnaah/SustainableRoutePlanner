using Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ServiceAgents
{
    public class EFAAgent : IServiceAgent
    {
        public async Task<IRouteResponse> GetRouteValues(ServiceAgentRequest routeReq)
        {
            Task<IRouteResponse> responseModelTask = SendRouteRequest(routeReq);
            IRouteResponse responseModel = await responseModelTask;

            return responseModel;
        }

        private static async Task<IRouteResponse> SendRouteRequest(ServiceAgentRequest routeRequest)
        {
            PublicTransportRouteResponse routeResponse = new PublicTransportRouteResponse(routeRequest.Id);

            string timeType = "";
            string date = "";
            string localTime = "";

            if(routeRequest.ArrivalTime == null && routeRequest.DepartureTime == null)
            {
                timeType = "dep";
                date = DateTime.Now.ToString("yyyyMMdd");
                localTime = DateTime.Now.ToString("HH:mm");
                routeResponse.DepartureTime = DateTime.Now;
            }
            else if (routeRequest.ArrivalTime != null)
            {
                timeType = "arr";
                date = routeRequest.ArrivalTime?.ToString("yyyyMMdd");
                localTime = routeRequest.ArrivalTime?.ToString("HH:mm");
                routeResponse.ArrivalTime = routeRequest.ArrivalTime;

            }
            else if (routeRequest.DepartureTime != null)
            {
                timeType = "dep";
                date = routeRequest.DepartureTime?.ToString("yyyyMMdd");
                localTime = routeRequest.DepartureTime?.ToString("HH:mm");
                routeResponse.DepartureTime = routeRequest.DepartureTime;
            }

            string getRequest = $"https://www.wienerlinien.at/ogd_routing/XML_TRIP_REQUEST2?locationServerActive=1&itdDate={date}&itdTime={localTime}&itdTripDateTimeDepArr={timeType}&ptOptionsActive=1&routeType=LEASTTIME&type_origin=any&name_origin={routeRequest.DepartureAdress}&anyObjFilter_origin=8&type_destination=any&name_destination={routeRequest.ArrivalAdress}&anyObjFilter_destination=8&outputFormat=JSON&coordOutputFormat=WGS84[DD.ddddd]";

            Debug.WriteLine($"EFA Trip Request: {getRequest}");


            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(getRequest);
            string responseBody = await response.Content.ReadAsStringAsync();

            Debug.WriteLine("Got response from EFA API");

            JObject obj = JsonConvert.DeserializeObject<JObject>(responseBody);


            //in hh:mm
            TimeSpan timeSpan = (TimeSpan)obj["trips"][0]["duration"];
            routeResponse.Duration = Math.Round(timeSpan.TotalMinutes, 2);


            foreach (JObject leg in obj["trips"][0]["legs"])
            {
                //calc distance

                List<double[]> coordsList = GetCoordinates((string)leg["path"]);

                double distance = 0;

                for(int i = 0; i<coordsList.Count; i++)
                {
                    if(i+1 == coordsList.Count)
                    {
                        break;
                    }
                    distance += CalcDistance(coordsList[i][0], coordsList[i][1], coordsList[i+1][0], coordsList[i+1][1]);
                }

                if ((string)leg["mode"]["product"] == "Fussweg")
                {
                    PublicTransportRouteManeuver direction = new PublicTransportRouteManeuver((string)leg["mode"]["product"], (string)leg["points"][0]["name"], (string)leg["points"][1]["name"], distance, (double)leg["timeMinute"]);
                    routeResponse.Maneuvers.Add(direction);
                }
                else
                {
                    PublicTransportRouteManeuver direction = new PublicTransportRouteManeuver((string)leg["mode"]["name"], (string)leg["mode"]["product"], distance, (double)leg["timeMinute"]);
                    
                    foreach(JObject stop in leg["stopSeq"])
                    {
                        direction.Stops.Add((string)stop["name"]);
                    }

                    routeResponse.Maneuvers.Add(direction);

                }
            }

            foreach(PublicTransportRouteManeuver maneuver in routeResponse.Maneuvers)
            {
                routeResponse.Distance += maneuver.Distance;
            }


            return routeResponse;
        }

        private static List<double[]> GetCoordinates(string coordsListString)
        {
            string[] coordsListStringArray = coordsListString.Split(' ');

            List <double[]> coordsDoubleList = new List<double[]>();

            foreach(string coordString in coordsListStringArray)
            {
                string[] coordsStringArray = coordString.Split(',');
                double[] coords = new double[2];

                //lat
                coords[0] = Double.Parse(coordsStringArray[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
                //lon
                coords[1] = Double.Parse(coordsStringArray[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

                coordsDoubleList.Add(coords);
            }


            return coordsDoubleList;
        }

        private static double CalcDistance(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; //Radius of earth in km

            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double x = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + 
                    Math.Cos(lat1 * Math.PI / 180) * 
                    Math.Cos(lat2 * Math.PI / 180) * 
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double d = 2 * Math.Asin(Math.Sqrt(x));

            double distance = R * d;

            return distance;
        }


        //double d = 2 * Math.Asin(Math.Min(1, Math.Sqrt(x)));

    }
}
