using System;
using System.Collections.Generic;

namespace Umove_AS.UI
{
    /// <summary>
    /// Holder styr på navigation i menu systemet, hvad for en side man er på og reager på indput
    /// Ved brug af IConsole
    /// </summary>
    public sealed class MenuManager
    {
        private readonly IConsole _con; //Konsol til input/output. _con kan indeholde alle objekter  fra IConsole
        private readonly Stack<MenuPage> _history = new(); //Laver en stack. Ex. lægger tallerkner oven på hinanden.
        private int _selected; //Holder styr på hvilket punkt brugeren har valgt.

        /// <summary>
        /// Constructoren
        /// </summary>
        /// <param name="console"></param>
        public MenuManager(IConsole console) => _con = console; // => betyder det samme som at man satte dem i { }
        //Dvs. det kunn have set sådan her ud:
        //  {
        //      _con = console
        //  }

        /// <summary> Push første side og start navigation‑løkken. </summary>
        public void Navigate(MenuPage root)
        {
            _history.Push(root); //Første menu side på stakken.

            while (_history.Count > 0) //Forsætter så længe der er sider i løkken.
            {
                var current = _history.Peek(); //Kig på den øverste side af stakken.
                ClampSelected(current.Items.Count); //Holder styr på antal menu punkter, og man bliver inde.

                UIMenu.DrawMenu(_con, current, _selected); //Tegner menuen i consollen.

                var key = _con.ReadKey(intercept: true).Key; //Læser tastetryk
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

        /// <summary>
        /// Sørger for at menuen går i ring.
        /// Går man ovre, eller under øverste eller nederste punkt
        /// Ryger man tilbage til toppen.
        /// </summary>
        /// <param name="count"></param>
        private void ClampSelected(int count)
        {
            if (count == 0) { _selected = 0; return; } // Hvis der ingen menu punkter er, går man tilbage til forige stack.
            if (_selected < 0) _selected = count - 1; // Hvis man står på øverste punkt og trykker og, ryger man til "bunden" af menuen.
            else if (_selected >= count) _selected = 0; //Hvis man står på nederste punkt, og trykker ned, ryger man til "toppen" af menuen.
        }
    }
}
