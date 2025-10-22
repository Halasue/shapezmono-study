using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    public static class Utils
    {
        /// <summary>
        /// 実行プラットフォーム名を取得
        /// </summary>
        /// <returns>プラットフォーム名</returns>
        public static string GetPlatformName()
        {
            if (OperatingSystem.IsWindows()) return "Windows";
            if (OperatingSystem.IsLinux()) return "Linux";
            if (OperatingSystem.IsMacOS()) return "macOS";
            return "Unknown";
        }
    }
}
