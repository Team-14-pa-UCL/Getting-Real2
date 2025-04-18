namespace Umove_AS.UI
{
    /// <summary> Minimal wrapper om System.Console så vi kan mocke i tests. </summary>
    public interface IConsole
    {
        void Clear();
        void WriteLine(string text);
        void ResetColor();
        ConsoleKeyInfo ReadKey(bool intercept);
        ConsoleColor ForegroundColor { get; set; }
        ConsoleColor BackgroundColor { get; set; }
    }

    public sealed class RealConsole : IConsole
    {
        public void Clear() => Console.Clear();
        public void WriteLine(string text) => Console.WriteLine(text);
        public void ResetColor() => Console.ResetColor();
        public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
        public ConsoleColor ForegroundColor { get => Console.ForegroundColor; set => Console.ForegroundColor = value; }
        public ConsoleColor BackgroundColor { get => Console.BackgroundColor; set => Console.BackgroundColor = value; }
    }
}
