using Models;
using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public interface IEmissionCalculator
    {
        EmissionFactorsLoader EmissionFactors { get; set; }

        string DepartureLocation { get; set; }
        string ArrivalLocation { get; set; }

        DateTime? DepartureTime { get; set; }
        DateTime? ArrivalTime { get; set; }

        Task<RouteResponse> CalcEmissions();
        Task<RouteResponse> GetRoute();

    }
}
