using Models;
using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public class EmissionCalculator
    {
        public EmissionFactorsLoader EmissionFactors { get; set; }

        public string DepartureLocation { get; set; }
        public string ArrivalLocation { get; set; }

        public DateTime? DepartureTime { get; set; }
        public DateTime? ArrivalTime { get; set; }


        public void CalcTime(ref RouteResponse response)
        {
            if (response.DepartureTime != null)
            {
                response.ArrivalTime = response.DepartureTime?.AddMinutes(response.Duration);
            }
            else if (response.ArrivalTime != null)
            {
                response.DepartureTime = response.ArrivalTime?.AddMinutes(-response.Duration);
            }
        }

    }
}
