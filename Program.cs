using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using ShapezMono.Game;

public static class Program
{
    [DllImport("kernel32.dll")]
    static extern bool AllocConsole();

    [STAThread]
    public static void Main()
    {
        AllocConsole();

        var app = new Application();
        _ = app.BootAsync();

        // 疑似的なメインループ
        while (true)
        {
            Thread.Sleep(16);
        }
    }
}