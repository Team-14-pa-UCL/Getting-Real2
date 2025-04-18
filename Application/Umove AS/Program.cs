using Umove_AS.UI;
using System.Collections.Generic;

namespace Umove_AS
{
    class Program
    {
        private static readonly MenuManager menu = new(new RealConsole());

        static void Main()
        {
            menu.Navigate(BuildMainMenu());
        }

        // ---------- menu‑fabrikker ----------

        private static MenuPage BuildMainMenu() => new(
            "UMOVE A/S",
            new List<MenuItem>
            {
                new ("Chauffør",        () => menu.Push(BuildDriverMenu())),
                new ("Driftmedarbejder",() => menu.Push(BuildOperationStaffMenu())),
                //new ("Exit",            () => Environment.Exit(0))
            });

        private static MenuPage BuildDriverMenu() => new(
            "Chaufør",
            new List<MenuItem>
            {
                new ("Indtast batteriprocent for bussen", PromptBatteryPercentage),
                new ("Fejlrapport: hurtig opladning",      PromptFastChargeError),
                //new ("Tilbage",                            () => 
            });

        private static MenuPage BuildOperationStaffMenu() => new(
            "Driftmedarbejder",
            new List<MenuItem>
            {
                new ("Administrer stam bus‑data",   PromptBusMasterData),
                new ("Overvåg batteristatus",       PromptBatteryMonitor),
                //new ("Tilbage",                     () => { /* Esc håndterer dette */ })
            });

        // ---------- “modale” prompts (dummy) ----------

        private static void PromptBatteryPercentage()
        {
            Console.Write("Angiv batteriprocent: ");
            var _ = Console.ReadLine();
            Console.WriteLine("Gemmer værdien...");
            Console.ReadKey();
        }

        private static void PromptFastChargeError()
        {
            Console.WriteLine("Fejlrapport registreret!");
            Console.ReadKey();
        }

        private static void PromptBusMasterData()
        {
            Console.WriteLine("Stamdata‑redigering (dummy).");
            Console.ReadKey();
        }

        private static void PromptBatteryMonitor()
        {
            Console.WriteLine("Viser batteriovervågning (dummy).");
            Console.ReadKey();
        }
    }
}
