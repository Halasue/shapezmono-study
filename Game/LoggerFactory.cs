using System;

namespace ShapezMono.Game
{
    /// <summary>
    /// ロガーを生成するファクトリクラス
    /// </summary>
    public static class LoggerFactory
    {
        public static Logger Create(object handle, ConsoleColor? color = null)
        {
            var context = handle?.GetType().Name ?? "Unknown";
            return new Logger(context, color ?? ConsoleColor.Gray);
        }
    }
}
