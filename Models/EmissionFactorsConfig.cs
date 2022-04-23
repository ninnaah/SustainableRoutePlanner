using Models.Transport;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class EmissionFactorsConfig
    {
        public GasCar GasCar { get; set; }
        public DieselCar DieselCar { get; set; }
        public ElectricCar ElectricCar { get; set; }
        public Bus Bus { get; set; }
        public PublicTransport PublicTransport { get; set; }
        public Bicycle Bicycle { get; set; }
        public EBike EBike { get; set; }
    }
}
