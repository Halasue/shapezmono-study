using System;
using System.IO;

namespace ShapezMono.Game.Core
{
    public static class Config
    {
        public const string DefaultWindowTitle = "Shapez Mono";

        public static readonly string UserData
            = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "shapez-mono");
    }
}
