using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapezMono.Game.Core
{
    public static class Compression
    {
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
