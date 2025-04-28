using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    public class Bus
    {
        public string ID { get; private set; }
        public double BatteryCapacity { get; private set; }
        public double Usage { get; private set; }

        public Bus(string id, double batterycapacity, double usage)
        {
            ID = id;
            BatteryCapacity = batterycapacity;
            Usage = usage;
        }

        public void Update(double newCapacity, double newUsage)
        {
            BatteryCapacity = newCapacity;
            Usage = newUsage;
        }
    }
}
