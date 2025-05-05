using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;
using Umove_AS.Type;

namespace Umove_AS.Services
{
    //Forretningslogik for garage-funktioner, kalder underliggende services.
    public class GarageService
    {
        private readonly BusService _busService;
        private readonly ShiftPlanService _shiftPlanService;

        public GarageService(BusService busService, ShiftPlanService shiftPlanService)
        {
            _busService = busService;
            _shiftPlanService = shiftPlanService;
        }

        public void CreateBus(string id, double batteryCapacity, double kmPerKWh, string shiftName)
        {
            var shiftPlan = _shiftPlanService.GetOrCreateShiftPlan(shiftName);
            var bus = new Bus(id, batteryCapacity, kmPerKWh, shiftPlan);
            _busService.AddBus(bus);
        }

        public void EditBus(string id, double newCapacity, double newUsage)
        {
            _busService.EditBus(id, newCapacity, newUsage);
        }

        public void UpdateBusStatus(string id, BusStatus newStatus)
        {
            _busService.UpdateBusStatus(id, newStatus);
        }

        public void DeleteBus(string id)
        {
            _busService.RemoveBus(id);
        }

        public List<Bus> GetAllBusses() => _busService.GetAllBusses();

        public void CreateShiftPlan(string shiftName)
        {
            _shiftPlanService.AddShiftPlan(new ShiftPlan(shiftName));
        }

        public void DeleteShiftPlan(string shiftName)
        {
            _shiftPlanService.RemoveShiftPlan(shiftName);
        }

        public List<ShiftPlan> GetAllShiftPlans() => _shiftPlanService.GetAllShiftPlans();
    }

}
