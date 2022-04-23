using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RouteResponseManeuver
    {
        public string Text { get; set; }
        public string Image { get; set; }

        public RouteResponseManeuver(string text, string image)
        {
            Text = text;
            Image = image;
        }
        public RouteResponseManeuver()
        {
        }
    }
}
