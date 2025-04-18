using System;
using System.Collections.Generic;

namespace Umove_AS.UI
{
    /// <summary>
    /// Styrer navigationen mellem menu‑sider og håndterer bruger‑input.
    /// </summary>
    public sealed class MenuManager
    {
        private readonly IConsole _con;
        private readonly Stack<MenuPage> _history = new();
        private int _selected;

        public MenuManager(IConsole console) => _con = console;

        /// <summary> Push første side og start navigation‑løkken. </summary>
        public void Navigate(MenuPage root)
        {
            _history.Push(root);

            while (_history.Count > 0)
            {
                var current = _history.Peek();
                ClampSelected(current.Items.Count);

                UIMenu.DrawMenu(_con, current, _selected);

                var key = _con.ReadKey(intercept: true).Key;
                switch (key)
                {
                    case ConsoleKey.UpArrow: _selected--; break;
                    case ConsoleKey.DownArrow: _selected++; break;

                    case ConsoleKey.Enter:
                        current.Items[_selected].Action?.Invoke();
                        _selected = 0; // reset til første punkt efter en action
                        break;

                    case ConsoleKey.Escape:
                        _history.Pop();         // gå ét niveau tilbage
                        _selected = 0;
                        break;
                }
            }
        }

        /// <summary> Skubber en ny side på stakken. </summary>
        public void Push(MenuPage page)
        {
            _history.Push(page);
        }

        private void ClampSelected(int count)
        {
            if (count == 0) { _selected = 0; return; }
            if (_selected < 0) _selected = count - 1;
            else if (_selected >= count) _selected = 0;
        }
    }
}
