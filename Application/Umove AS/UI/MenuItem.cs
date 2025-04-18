namespace Umove_AS.UI
{
    /// <summary> Utforsk‑venligt, immutabelt menupunkt. </summary>
    public readonly record struct MenuItem(string Text, Action Action);
}