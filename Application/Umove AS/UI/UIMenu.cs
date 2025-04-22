using System;
using System.Collections.Generic;
using System.Linq;

namespace Umove_AS.UI
{
    /// <summary> UIMenu indholder kun ting om der skal vises.. </summary>
    public static class UIMenu
    {
        public static void DrawMenu(IConsole con, MenuPage page, int selectedIndex)
        {
            con.Clear(); //Skærmen rydes

            // Tegner titelbox hvis der er titel -> metode længere nede.
            if (!string.IsNullOrEmpty(page.Title)) 
            {
                DrawTitleBox(con, page.Title);
            }

            int boxWidth = page.Items.Max(i => i.Text.Length) + 10; //Finder den længste tekst i menuen, og ligger 10 oven i
            con.WriteLine("╔" + new string('═', boxWidth) + "╗"); //Tegner øverste ramme

            //Tegner hvert menupunkt, en linje ad gangen
            for (int i = 0; i < page.Items.Count; i++)
            {
                string line = $"║  {page.Items[i].Text.PadRight(boxWidth - 4)}  ║";

                if (i == selectedIndex) //Giver farven til det markerede felt.
                {
                    con.BackgroundColor = ConsoleColor.DarkBlue;
                    con.ForegroundColor = ConsoleColor.White;
                }

                con.WriteLine(line);
                con.ResetColor();
            }

            con.WriteLine("╚" + new string('═', boxWidth) + "╝"); //Tegner nederste ramme
            con.WriteLine("\n↑/↓ = flyt, Enter = vælg, Esc = tilbage"); //Forklaring til brug
        }

        //Hvis der er en titel box, bliver boxen lavet her.
        private static void DrawTitleBox(IConsole con, string title)
        {
            int width = title.Length + 4;
            con.ForegroundColor = ConsoleColor.Yellow;
            con.WriteLine($"╔{new string('═', width)}╗");
            con.WriteLine($"║  {title}  ║");
            con.WriteLine($"╚{new string('═', width)}╝");
            con.ResetColor();
        }
    }
}
