using Models.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface IRouteResponse
    {
        DateTime? DepartureTime { get; set; }
        DateTime? ArrivalTime { get; set; }

        double Duration { get; set; } //min
        double Distance { get; set; } //km

        RouteEmissions RouteEmissions { get; set; }
        Guid Id { get; set; }
    }
}
