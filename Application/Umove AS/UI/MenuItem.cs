using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Umove_AS.UI
{
    public class MenuItem
    {
        // Teksten, der vises for dette menupunkt.
        public string Text { get; }

        // En handling (metode), der udføres, når dette menupunkt vælges.
        public Action Action { get; }

        // Constructor til at oprette et menupunkt.
        // 'text' er den tekst, der vises i menuen.
        // 'action' er den metode, der skal udføres, når brugeren vælger dette menupunkt.
        public MenuItem(string text, Action action)
        {
            Text = text; // Initialiserer tekstfeltet.
            Action = action; // Initialiserer handlingen.
        }
    }

}
