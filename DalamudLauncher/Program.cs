using Common.Models;
using Common.Utility;
using DalamudLauncher.Patching;
using Newtonsoft.Json;


namespace DalamudLauncher
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                ServerInfoModel serverInfoModel = Utils.Instance.GetSelectedServer();
                
                if (args[0].Contains("ffxivlogin"))
                {
                    LoginPatching loginPatching = new (args,serverInfoModel);
                    loginPatching.ApplyPatches();
                }else if (args[0].Contains("ffxivgame"))
                {
                    GamePatching gamePatching = new (args, serverInfoModel);
                    gamePatching.LaunchGame();
                }
            }
            else
            {
                Console.WriteLine("Please run the UI app");
            }
        }
        
    }
}