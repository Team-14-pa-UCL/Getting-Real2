using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;

namespace Umove_AS.Services
{
    //Håndtering af vagtplaner
    public class ShiftPlanService
    {
        private readonly List<ShiftPlan> _shiftPlans = new(); // Interne vagtplaner

        public void AddShiftPlan(ShiftPlan plan) => _shiftPlans.Add(plan);
        public void RemoveShiftPlan(string name)
        {
            var plan = _shiftPlans.FirstOrDefault(p => p.ShiftName == name);
            if (plan == null) throw new ArgumentException("Vagtplan ikke fundet.");
            _shiftPlans.Remove(plan);
        }
        public List<ShiftPlan> GetAllShiftPlans() => _shiftPlans;
        public ShiftPlan GetOrCreateShiftPlan(string name)
        {
            var existing = _shiftPlans.FirstOrDefault(p => p.ShiftName == name);
            if (existing != null) return existing;
            var newPlan = new ShiftPlan(name);
            _shiftPlans.Add(newPlan);
            return newPlan;
        }
    }
}
