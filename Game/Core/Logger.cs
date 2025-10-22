using System;

namespace ShapezMono.Game.Core
{
    /// <summary>
    /// ロガークラス
    /// </summary>
    public class Logger
    {
        private readonly string _context;
        private readonly ConsoleColor _color;

        public Logger(string context, ConsoleColor color = ConsoleColor.Gray)
        {
            _context = context.PadRight(20);
            _color = color;
        }

        public void Log(string message)
        {
            WriteWithColor(message, ConsoleColor.White);
        }

        public void Warn(string message)
        {
            WriteWithColor("⚠ " + message, ConsoleColor.Yellow);
        }

        public void Error(string message)
        {
            WriteWithColor("✖ " + message, ConsoleColor.Red);
        }

        private void WriteWithColor(string message, ConsoleColor color)
        {
            var prev = Console.ForegroundColor;

            Console.ForegroundColor = _color;
            Console.Write($"[{_context}] ");

            Console.ForegroundColor = color;
            Console.WriteLine(message);

            Console.ForegroundColor = prev;
        }

    }
}
