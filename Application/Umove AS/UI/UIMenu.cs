using System;
using System.Collections.Generic;
using System.Linq;

namespace Umove_AS.UI
    {
        /// <summary>
        /// Indeholder statiske hjælpe­metoder til at tegne en simpel
        /// tekst‑baseret menu i konsollen.
        /// </summary>
        public static class UIMenu
        {
            /// <summary>
            /// Tegner menuen i konsollen.
            /// </summary>
            /// <param name="items">Listen af menupunkter som skal vises.</param>
            /// <param name="selectedIndex">Det aktuelle (markerede) indeks i listen.</param>
            /// <param name="title">Valgfri titel der vises som en boks over menuen.</param>
            public static void DrawMenu(List<MenuItem> items, int selectedIndex, string title = null)
            {
                Console.Clear();                         // Ryd skærmen før vi tegner noget nyt.

                if (!string.IsNullOrEmpty(title))       // Tegn titel­boksen hvis vi har fået en titel.
                {
                    DrawTitleBox(title);
                }

                // Beregn hvor bred menu­boksen skal være:
                //  * længste tekst i listen
                //  * + 10 ekstra tegn (to mellem­rum + margener + bokskanter).
                int boxWidth = items.Max(i => i.Text.Length) + 10;

                // Øverste kant af menu­boksen.
                Console.WriteLine("╔" + new string('═', boxWidth) + "╗");

                // Gå igennem alle menupunkter …
                for (int i = 0; i < items.Count; i++)
                {
                    // Sammensæt linjen: ”║␣␣tekst␣(pad)␣␣║”
                    string line = $"║  {items[i].Text.PadRight(boxWidth - 4)}  ║";

                    if (i == selectedIndex)             // Er dette punktet, der er markeret?
                    {
                        // Skift baggrund og forgrund for at fremhæve valget.
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine(line);            // Skriv linjen.
                    Console.ResetColor();               // Gendan standard­farverne.
                }

                // Nederste kant af menu­boksen.
                Console.WriteLine("╚" + new string('═', boxWidth) + "╝");
            }

            /// <summary>
            /// Tegner en fremhævet titelboks over menuen.
            /// </summary>
            private static void DrawTitleBox(string title)
            {
                int width = title.Length + 4;           // To mellem­rum på hver side af titlen.

                Console.ForegroundColor = ConsoleColor.Yellow;  // Gør teksten gul for synlighed.

                Console.WriteLine($"╔{new string('═', width)}╗"); // Øverste kant
                Console.WriteLine($"║  {title}  ║");              // Selve titlen
                Console.WriteLine($"╚{new string('═', width)}╝"); // Nederste kant

                Console.ResetColor();                   // Gendan farverne.
            }
        }
    }


