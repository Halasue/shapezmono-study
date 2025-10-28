using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using ShapezMono.Game.old.Core;

namespace ShapezMono.Game.old
{
    /// <summary>
    /// メインゲームクラス
    /// </summary>
    public class Application
    {
        public ErrorHandler? ErrorHandler { get; private set; }

        private Storage? _storage;

        private Logger _logger = LoggerFactory.Create("Application");

        public async Task BootAsync()
        {
            Console.WriteLine("ShapezMono 起動中...");

            ErrorHandler = new ErrorHandler();

            _logger.Log("Applicationを作成中...");

            _storage = new Storage(this, Storage.STORAGE_SAVES);
            await _storage.InitializeAsync();

        }
    }
}
