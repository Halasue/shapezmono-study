using System.IO;

namespace ShapezMono.Game.Core
{
    public class Storage
    {
        public const string STORAGE_SAVES = "saves";

        private readonly Application _app;

        private readonly string _rootDir;
        private bool _initialized = false;

        public Storage(Application app, string subDir)
        {
            _app = app;
            _rootDir = Path.Combine(Config.UserData, subDir);
        }

        public void Initialize()
        {
            if (_initialized) return;
            Directory.CreateDirectory(_rootDir);
            _initialized = true;
        }

        //public async Task WriteFileAsync(string fileName, object contents)
        //{
        //}
    }
}
