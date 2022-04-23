using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmissionCalculator
{
    public class EmissionFactorsLoader
    {
        public EmissionFactors EmissionFactors { get; set; }

        public void LoadEmissionFactors()
        {
            EmissionFactors = JsonConvert.DeserializeObject<EmissionFactors>(File.ReadAllText(@"../EmissionCalculator/emissionFactors.json"));
            Debug.WriteLine($"Loaded emissionFactors");
        }
    }
}
