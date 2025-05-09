using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS_WPF.MVVM.Models;
using Umove_AS_WPF.MVVM.Type;

namespace Umove_AS_WPF.Services
{
    //Administrere listen af busser
    public class BusService
    {
        private readonly List<Bus> _buses = new(); // Intern liste over busser

        public void AddBus(Bus bus) => _buses.Add(bus);
        public void RemoveBus(string id)
        {
            var bus = GetBusById(id);
            if (bus != null)
                _buses.Remove(bus);
        }
        public void EditBus(string id, int newBatteryLevel, int newKilometers)
        {
            var bus = GetBusById(id);
            if (bus != null)
            {
                bus.BatteryLevel = newBatteryLevel;
                bus.Kilometers = newKilometers;
            }
        }
        public void UpdateBusStatus(string id, BusStatus newStatus)
        {
            var bus = GetBusById(id);
            if (bus == null) throw new ArgumentException("Bus ikke fundet.");
            bus.UpdateStatus(newStatus);
        }

        public Bus GetBusById(string id) => _buses.FirstOrDefault(b => b.BusId == id);
        public List<Bus> GetAllBusses() => _buses;
    }
}
