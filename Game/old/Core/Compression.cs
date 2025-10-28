using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapezMono.Game.old.Core
{
    /// <summary>
    /// データ圧縮ユーティリティ
    /// </summary>
    public static class Compression
    {
        /// <summary>
        /// データを圧縮する
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<byte[]> CompressAsync(object data)
        {
            var json = JsonSerializer.Serialize(data);
            var raw = Encoding.UTF8.GetBytes(json);

            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
            {
                await gzip.WriteAsync(raw, 0, raw.Length);
            }

            return output.ToArray();
        }

        /// <summary>
        /// データを展開する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<T?> DecompressAsync<T>(byte[] data)
        {
            using var input = new MemoryStream(data);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var reader = new StreamReader(gzip, Encoding.UTF8);
            var json = await reader.ReadToEndAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
