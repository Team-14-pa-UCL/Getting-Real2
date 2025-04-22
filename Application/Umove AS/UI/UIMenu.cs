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
            con.Clear();

            var titleLines = string.IsNullOrEmpty(page.Title) ? new string[0] : page.Title.Split('\n');
            int titleWidth = titleLines.Any() ? titleLines.Max(l => l.Length) : 0;
            int menuWidth = page.Items.Any() ? page.Items.Max(i => i.Text.Length) + 10 : 0;

            int boxWidth = Math.Max(titleWidth, menuWidth);

            // Tegn titel (hvis den findes) med fælles bredde
            if (titleLines.Any())
                DrawTitleBox(con, page.Title, boxWidth);

            // Tegn menu-ramme
            con.WriteLine($"╔{new string('═', boxWidth)}╗");

            for (int i = 0; i < page.Items.Count; i++)
            {
                string text = page.Items[i].Text;
                string line = $"║  {text.PadRight(boxWidth - 4)}  ║";

                if (i == selectedIndex)
                {
                    con.BackgroundColor = ConsoleColor.DarkBlue;
                    con.ForegroundColor = ConsoleColor.White;
                }

                con.WriteLine(line);
                con.ResetColor();
            }

            con.WriteLine($"╚{new string('═', boxWidth)}╝");

            con.WriteLine("\n↑/↓ = flyt, Enter = vælg, Esc = tilbage");
        }

        private static void DrawTitleBox(IConsole con, string title, int boxWidth)
        {
            var lines = title.Split('\n');                      // Del op i linjer
            con.ForegroundColor = ConsoleColor.Yellow;
            con.WriteLine($"╔{new string('═', boxWidth)}╗");

            foreach (var line in lines)
            {
                int padding = boxWidth - line.Length;
                int padLeft = padding / 2;                              // 2 for kanten og mellemrum
                int padRight = boxWidth - padLeft - line.Length;        // Resten til højre

                string centeredLine = "║" + new string(' ', padLeft) + line + new string(' ', padRight) + "║";
                con.WriteLine(centeredLine);
            }

            con.WriteLine($"╚{new string('═', boxWidth)}╝");
            con.ResetColor();
        }
    }
}
