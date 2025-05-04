using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Type
{
    public enum BusStatus //Laver den public 04-05-2025
    {
        Inroute,
        Intercept, //Ny 04-05-2025 tænkte også der skal være en status for, hvis den er på vej til at erstatte en anden bus.
        Free,
        Garage,
        Charging,
        Repair
    }
}
