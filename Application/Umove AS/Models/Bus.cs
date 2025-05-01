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
        public double KmPerKwh { get; private set; } //DK
        public double CurrentCharge { get; private set; } //DK
        public string Location { get; private set; } //DK
        


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

        public void GetBatteryStatus()
        {
        }

        public void FilterBatteryStatus()
        {
        }

        public void GetBusStatus()
        {
        }

        public void GetBatteryTimeLeft()
        {
        }

        public void GetChargingTimeLeft()
        {
        }

        public void SortByCurrentCharge()
        {
        }
    }
}
