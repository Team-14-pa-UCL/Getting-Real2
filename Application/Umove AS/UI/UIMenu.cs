using System;
using System.Collections.Generic;
using System.Linq;

namespace Umove_AS.UI
{
    public static class UIMenu
    {
        public static void DrawMenu(IConsole con, MenuPage page, int selectedIndex)
        {
            con.Clear();

            // --- tegn titelboks ---
            if (!string.IsNullOrEmpty(page.Title))
            {
                DrawTitleBox(con, page.Title);
            }

            int boxWidth = page.Items.Max(i => i.Text.Length) + 10;
            con.WriteLine("╔" + new string('═', boxWidth) + "╗");

            for (int i = 0; i < page.Items.Count; i++)
            {
                string line = $"║  {page.Items[i].Text.PadRight(boxWidth - 4)}  ║";

                if (i == selectedIndex)
                {
                    con.BackgroundColor = ConsoleColor.DarkBlue;
                    con.ForegroundColor = ConsoleColor.White;
                }

                con.WriteLine(line);
                con.ResetColor();
            }

            con.WriteLine("╚" + new string('═', boxWidth) + "╝");
            con.WriteLine("\n↑/↓ = flyt, Enter = vælg, Esc = tilbage");
        }

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
