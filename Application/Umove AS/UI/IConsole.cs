namespace Umove_AS.UI
{
    /// <summary> En kontrak på funktioner i enhver konsol/side i systemet skal bruge <summary>
    public interface IConsole
    {
        void Clear();
        void WriteLine(string text);
        void ResetColor();
        ConsoleKeyInfo ReadKey(bool intercept);
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
    }

    /// <summary> Implementering af interface IConsole </summary>
    public sealed class RealConsole : IConsole // : betyder implementer interface Iconsole
    {
        public void Clear() => Console.Clear();
        public void WriteLine(string text) => Console.WriteLine(text);
        public void ResetColor() => Console.ResetColor();
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
    }
}
