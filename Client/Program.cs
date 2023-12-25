using DungeonFrame;
using System.IO;
using System.Threading;

//if (Directory.Exists(World.WorldDataPath))
//{
//    Directory.Delete(World.WorldDataPath, true);
//}
//
//World.EnsureWorldDataFolderExists();
//
//Thread.Sleep(1000);

using var game = new Client.Game1();
game.Run();
