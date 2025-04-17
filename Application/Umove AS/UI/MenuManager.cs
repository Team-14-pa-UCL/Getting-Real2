using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.UI
{
    public class MenuManager
    {
        // Stack til at holde styr på menu-hierarkiet. Hver menu er en liste af MenuItem-objekter.
        private Stack<List<MenuItem>> menuStack = new();

        // Viser en ny menu og tilføjer den til stacken.
        public void ShowMenu(List<MenuItem> menu, string title = "")
        {
            menuStack.Push(menu); // Tilføjer den nye menu til toppen af stacken.
            RunCurrentMenu(title); // Kører den aktuelle menu.
        }

        // Kører den menu, der er øverst i stacken.
        private void RunCurrentMenu(string title = "")
        {
            var currentMenu = menuStack.Peek(); // Henter den øverste menu fra stacken uden at fjerne den.
            int selectedIndex = 0; // Holder styr på det aktuelt valgte menupunkt.
            bool inMenu = true; // Bruges til at holde menuen åben, indtil brugeren vælger at afslutte.

            while (inMenu)
            {
                // Tegner menuen på skærmen med det aktuelle valg markeret.
                UIMenu.DrawMenu(currentMenu, selectedIndex, title);

                // Læser brugerens tastetryk.
                ConsoleKey key = Console.ReadKey(true).Key;

                // Håndterer brugerens input.
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        // Flytter markeringen opad i menuen. Hvis vi er på det første element, går vi til det sidste.
                        selectedIndex = (selectedIndex - 1 + currentMenu.Count) % currentMenu.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        // Flytter markeringen nedad i menuen. Hvis vi er på det sidste element, går vi til det første.
                        selectedIndex = (selectedIndex + 1) % currentMenu.Count;
                        break;
                    case ConsoleKey.Enter:
                        // Når brugeren trykker Enter, udføres den valgte menuitems handling.
                        Console.Clear(); // Rydder konsollen.
                        currentMenu[selectedIndex].Action.Invoke(); // Udfører den tilknyttede handling.
                        inMenu = false; // Afslutter menuen.
                        break;
                }
            }

            // Når menuen afsluttes, fjernes den fra stacken.
            menuStack.Pop();
        }

        // Går tilbage til den forrige menu i stacken.
        public void GoBack()
        {
            if (menuStack.Count > 1) // Tjekker, om der er en tidligere menu at gå tilbage til.
            {
                menuStack.Pop(); // Fjerner den aktuelle menu fra stacken.
                RunCurrentMenu(); // Kører den forrige menu.
            }
            else
            {
                // Hvis der ikke er nogen tidligere menu, sendes brugeren til hovedmenuen.
                Program.ShowMainMenu(); // Fallback til hovedmenuen.
            }
        }

        // Rydder hele menu-stacken.
        public void ClearMenuStack()
        {
            menuStack.Clear(); // Fjerner alle menuer fra stacken.
        }
    }
}
