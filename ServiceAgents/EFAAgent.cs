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
using NetTopologySuite.Geometries;
using System.Globalization;

namespace ServiceAgents
{
    public class EFAAgent
    {
        public async Task<PublicTransportRouteResponse> GetRouteValues(ServiceAgentRequest routeReq)
        {
            Task<PublicTransportRouteResponse> responseModelTask = SendRouteRequest(routeReq);
            PublicTransportRouteResponse responseModel = await responseModelTask;

            return responseModel;
        }

        private static async Task<PublicTransportRouteResponse> SendRouteRequest(ServiceAgentRequest routeRequest)
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

            //routeResponse.Distance = (double)obj["trips"][0]["distance"]/1000;


            foreach (JObject leg in obj["trips"][0]["legs"])
            {
                //calc distance
                double[] firstCoords = GetCoordinates((string)leg["points"][0]["ref"]["coords"]);
                double[] secondCoords = GetCoordinates((string)leg["points"][1]["ref"]["coords"]);
                double distance = DistanceBetweenPlaces(firstCoords[1], firstCoords[0], secondCoords[1], secondCoords[0]);

                if ((string)leg["mode"]["product"] == "Fussweg")
                {
                    PublicTransportRouteManeuver direction = new PublicTransportRouteManeuver((string)leg["mode"]["product"], distance, (double)leg["timeMinute"]);
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

        public static double[] GetCoordinates(string responseCoordinates)
        {
            string[] coordsArray = responseCoordinates.Split(',');
            double[] coords = new double[2];

            //lat
            coords[0] = Double.Parse(coordsArray[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);
            //lon
            coords[1] = Double.Parse(coordsArray[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            return coords;
        }

        //https://stackoverflow.com/questions/6544286/calculate-distance-of-two-geo-points-in-km-c-sharp
        public static double DistanceBetweenPlaces(double lon1, double lat1, double lon2, double lat2)
        {
            double R = 6371; // km

            double sLat1 = Math.Sin(lat1 * Math.PI / 180);
            double sLat2 = Math.Sin(lat2 * Math.PI / 180);
            double cLat1 = Math.Cos(lat1 * Math.PI / 180);
            double cLat2 = Math.Cos(lat2 * Math.PI / 180);
            double cLon = Math.Cos((lon1 * Math.PI / 180) - (lon2 * Math.PI / 180));

            double cosD = sLat1 * sLat2 + cLat1 * cLat2 * cLon;

            double d = Math.Acos(cosD);

            double dist = R * d;

            return dist;
        }


    }
}
