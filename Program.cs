using System;
using System.Runtime.InteropServices;

[DllImport("kernel32.dll")]
static extern bool AllocConsole();

AllocConsole(); // コンソールを開く
Console.WriteLine("ゲーム起動中...");

using var game = new ShapezMono.Game.Application();

game.Run();