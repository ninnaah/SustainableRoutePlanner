using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public interface IEmissionCalculator
    {
        string DepartureLocation { get; set; }
        string ArrivalLocation { get; set; }

        DateTime? DepartureTime { get; set; }
        DateTime? ArrivalTime { get; set; }

        void LoadEmissionFactors();
        void CalcEmissions();

        Task<RouteResponse> GetRoute();

    }
}
