using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umove_AS.Models;
using Umove_AS.Services;
using Umove_AS.Type;

namespace Umove_AS.UI
{
    // UI-lag der tager sig af brugerindput/output for garage funktioner.
    public class GarageUI
    {
        private readonly GarageService _garageService;

        public GarageUI(GarageService garageService)
        {
            _garageService = garageService;
        }

        public void CreateBus()
        {
            Console.Write("ID: ");
            var id = Console.ReadLine();
            Console.Write("Batterikapacitet: ");
            var capacity = double.Parse(Console.ReadLine());
            Console.Write("Forbrug (km/kWh): ");
            var usage = double.Parse(Console.ReadLine());
            Console.Write("Vagtplan: ");
            var shift = Console.ReadLine();

            _garageService.CreateBus(id, capacity, usage, shift);
            Console.WriteLine("Bus oprettet.");
            Console.ReadKey();
        }

        public void EditBus()
        {
            Console.Write("ID på bus der skal redigeres: ");
            var id = Console.ReadLine();
            Console.Write("Ny batterikapacitet: ");
            var capacity = double.Parse(Console.ReadLine());
            Console.Write("Nyt forbrug (km/kWh): ");
            var usage = double.Parse(Console.ReadLine());

            _garageService.EditBus(id, capacity, usage);
            Console.WriteLine("Bus opdateret.");
            Console.ReadKey();
        }

        public void UpdateBusStatus()
        {
            Console.Write("ID på bus: ");
            var id = Console.ReadLine();
            Console.WriteLine("Vælg ny status (InRoute, Intercept, Returning, Free, Garage, Charging, Repair): ");
            var statusInput = Console.ReadLine();

            if (Enum.TryParse(statusInput, out BusStatus newStatus))
            {
                _garageService.UpdateBusStatus(id, newStatus);
                Console.WriteLine("Busstatus opdateret.");
            }
            else
            {
                Console.WriteLine("Ugyldig status indtastet.");
            }
            Console.ReadKey();
        }

        public void DeleteBus()
        {
            Console.Write("ID på bus der skal slettes: ");
            var id = Console.ReadLine();
            _garageService.DeleteBus(id);
            Console.WriteLine("Bus slettet.");
            Console.ReadKey();
        }

        public void ShowBusses()
        {
            var buses = _garageService.GetAllBusses();
            foreach (var bus in buses)
            {
                Console.WriteLine(bus.GetBusStatus());
            }
            Console.ReadKey();
        }

        public void CreateShiftPlan()
        {
            Console.Write("Vagtplan navn: ");
            var name = Console.ReadLine();
            _garageService.CreateShiftPlan(name);
            Console.WriteLine("Vagtplan oprettet.");
            Console.ReadKey();
        }

        public void DeleteShiftPlan()
        {
            Console.Write("Navn på vagtplan der skal slettes: ");
            var name = Console.ReadLine();
            _garageService.DeleteShiftPlan(name);
            Console.WriteLine("Vagtplan slettet.");
            Console.ReadKey();
        }

        public void ShowShiftPlans()
        {
            var plans = _garageService.GetAllShiftPlans();
            foreach (var plan in plans)
            {
                Console.WriteLine($"Vagtplan: {plan.ShiftName}");
            }
            Console.ReadKey();
        }
        public void SelectBus()
        {
            var buses = _garageService.GetAllBusses();


            Console.Write("Indtast BusID: ");
            var id = Console.ReadLine();

            Bus selectedBus = buses.FirstOrDefault(bus => bus.ID == id);

            if (selectedBus != null)
            {
                //Console.WriteLine($"Selected Bus: {selectedBus}"); // Debug statement

                // Select current weather
                selectedBus.SelectCurrentWeather();
            }
            else
            {
                Console.WriteLine("Ugyldigt valg. Vælg venligst en gyldig bus.");
            }

            Console.ReadKey();

        }
    }


}
