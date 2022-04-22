using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Transport
{
    public class Car : IVehicle
    {
        public CarType Type { get; set; }

        public Car(CarType type)
        {
            Type = type;
        }
    }
}
