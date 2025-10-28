using System;

namespace ShapezMono.Game
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

        /// <summary>
        /// ログを出力する
        /// </summary>
        /// <param name="message"></param>
        public void Log(string message)
        {
            WriteWithColor(message, ConsoleColor.White);
        }

        /// <summary>
        /// 警告ログを出力する
        /// </summary>
        /// <param name="message"></param>
        public void Warn(string message)
        {
            WriteWithColor("⚠ " + message, ConsoleColor.Yellow);
        }

        /// <summary>
        /// エラーログを出力する
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            WriteWithColor("✖ " + message, ConsoleColor.Red);
        }

        /// <summary>
        /// 色付きでログを出力する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="color"></param>
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
