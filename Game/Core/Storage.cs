using System;
using System.IO;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    public class Storage
    {
        public const string STORAGE_SAVES = "saves";

        private readonly Application _app;

        private readonly string _id;
        private readonly string _rootDir;
        private bool _initialized = false;

        public Storage(Application app, string id)
        {
            _id = id;
            _app = app;
            _rootDir = Path.Combine(Config.UserData, id);
        }

        /// <summary>
        /// 初期化
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            if (_initialized) return;

            // ユーザーがファイルの置き場所を認識できるように、ディレクトリを作成しておく
            Directory.CreateDirectory(_rootDir);
            _initialized = true;

            await Task.CompletedTask;
        }

        /// <summary>
        /// ファイルを非同期で読み込む
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task<TData?> ReadFileAsync<TData>(string fileName)
        {
            var bytes = await File.ReadAllBytesAsync(fileName);
            var decompressed = await Compression.DecompressAsync<TData>(bytes);
            return decompressed;
        }

        /// <summary>
        /// ファイルを非同期で書き込む
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public async Task WriteFileAsync<TData>(string fileName, TData contents)
        {
            var parentDir = Path.GetDirectoryName(fileName);
            Directory.CreateDirectory(parentDir ?? ".");
            if (contents == null) throw new ArgumentNullException(nameof(contents));
            var compressed = 
                await Compression.CompressAsync(contents);
            await File.WriteAllBytesAsync(fileName, compressed);
        }

        /// <summary>
        /// ファイルを非同期で削除する
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public async Task DeleteFileAsync(string fileName)
        {
            await Task.Run(() =>
            {
                // unlinkのように、ファイルが存在しない場合は例外を投げる
                if (!File.Exists(fileName))
                    throw new FileNotFoundException($"ファイルが見つかりません: {fileName}");
                File.Delete(fileName);
            });
        }
    }
}
