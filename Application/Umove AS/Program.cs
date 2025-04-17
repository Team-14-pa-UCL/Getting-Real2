using Umove_AS.UI;
using System;
using System.Collections.Generic;

namespace Umove_AS
{
    /// <summary>
    /// Program­klassen fungerer som entry‑point og central menu‑controller
    /// for det tekst‑baserede UI.
    /// </summary>
    class Program
    {
        // Én delt MenuManager-instans styrer navigationen (stack med "tilbage"‑historik).
        static readonly MenuManager menuManager = new();

        /// <summary>
        /// Applikationens entry‑point (CLR leder efter Main).
        /// </summary>
        static void Main()
        {
            ShowMainMenu();   // Vis startmenuen og overlader kontrol til MenuManager.
        }

        /// <summary>
        /// Sammensætter og viser hovedmenuen.
        /// </summary>
        public static void ShowMainMenu()
        {
            List<MenuItem> mainMenu = new()
            {
                // Hver MenuItem tager tekst + en Action‑dele­gat der kaldes ved valg.
                new MenuItem("Chaufør", DriverMenu),            // TODO: Kunne blive et login‑punkt.
                new MenuItem("Driftmedarbejder", OperationStaffMenu),
                new MenuItem("Exit", () => Environment.Exit(0))
            };

            menuManager.ShowMenu(mainMenu, "UMOVE A/S");   // Tegner via UIMenu og håndterer input.
        }

        /// <summary>
        /// Menuen som chauføren ser efter login/valg.
        /// </summary>
        public static void DriverMenu()
        {
            List<MenuItem> driverMenu = new()
            {
                new MenuItem("Indtast bateriprocent for bussen", () =>
                {
                    Console.WriteLine("Her kaldes senere en metode …");
                    Console.ReadKey();

                    menuManager.ClearMenuStack();  // “Frisk start” når vi går tilbage til DriverMenu.
                    DriverMenu();                  // Tegn menuen igen.
                }),
                new MenuItem("Fejl Rapporter hurtig opladning", () =>
                {
                    Console.WriteLine("Her kaldes en anden metode …");
                    Console.ReadKey();

                    menuManager.ClearMenuStack();
                    DriverMenu();
                }),
                new MenuItem("Tilbage", () => menuManager.GoBack())
            };

            menuManager.ShowMenu(driverMenu, "Driver Menu");
        }

        /// <summary>
        /// Menuen for drift­medarbejdere.
        /// </summary>
        public static void OperationStaffMenu()
        {
            List<MenuItem> operationStaffMenu = new()
            {
                new MenuItem("Administrer stam bus data", () =>
                {
                    Console.WriteLine("Her skal stamdata‑funktionalitet ligge …");
                    Console.ReadKey();

                    menuManager.ClearMenuStack();
                    OperationStaffMenu();
                }),
                new MenuItem("Overvågning af batteristatus", () =>
                {
                    Console.WriteLine("Her skal overvågnings‑view ligge …");
                    Console.ReadKey();

                    menuManager.ClearMenuStack();
                    OperationStaffMenu();
                }),
                new MenuItem("Tilbage", () => menuManager.GoBack())
            };

            menuManager.ShowMenu(operationStaffMenu, "Driftmedarbejder");
        }
    }
}
