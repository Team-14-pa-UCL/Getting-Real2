using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;
using Umove_AS.Type;

namespace Umove_AS.Services
{
    //Administrere listen af busser
    public class BusService
    {
        private readonly List<Bus> _buses = new(); // Intern liste over busser

        public void AddBus(Bus bus) => _buses.Add(bus);
        public void RemoveBus(string id)
        {
            var bus = GetBusById(id);
            if (bus == null) throw new ArgumentException("Bus ikke fundet.");
            _buses.Remove(bus);
        }
        public void EditBus(string id, double newCapacity, double newUsage)
        {
            var bus = GetBusById(id);
            if (bus == null) throw new ArgumentException("Bus ikke fundet.");
            bus.Update(newCapacity, newUsage);
        }
        public void UpdateBusStatus(string id, BusStatus newStatus)
        {
            var bus = GetBusById(id);
            if (bus == null) throw new ArgumentException("Bus ikke fundet.");
            bus.UpdateStatus(newStatus);
        }

        public Bus GetBusById(string id) => _buses.FirstOrDefault(b => b.ID == id);
        public List<Bus> GetAllBusses() => _buses;
    }
}
