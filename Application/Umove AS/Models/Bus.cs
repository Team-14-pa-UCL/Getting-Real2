using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.Models
{
    enum BusStatus
    {
        Inroute,
        Free,
        Garage,
        Charging,
        Repair
    }
    public class Bus
    {
        public string ID { get; private set; }
        public double BatteryCapacity { get; private set; }
        public double KmPerKWh { get; private set; } //DK(nyt property navn)
        public double CurrentCharge { get; private set; } //DK
        public string Location { get; private set; } //DK
        


        public Bus(string id, double batterycapacity, double kmPerKWh)
        {
            ID = id;
            BatteryCapacity = batterycapacity;
            KmPerKWh = kmPerKWh;
        }

        public void Update(double newCapacity, double newKmPerKWh)
        {
            BatteryCapacity = newCapacity;
            KmPerKWh = newKmPerKWh;
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
