using System.Runtime.InteropServices;
using ShapezMono.Game;

[DllImport("kernel32.dll")]
static extern bool AllocConsole();

AllocConsole();
var game = new MainGame();
game.Run();
