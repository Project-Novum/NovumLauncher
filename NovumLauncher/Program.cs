using Common.Models;
using Common.Patching;
using NovumLauncher.Patching;

namespace NovumLauncher
{
    public static class Program
    {
        
        public static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                ServerInfoModel serverInfoModel = Common.Utility.Utils.Instance.GetSelectedServer();

                if (args[0].Contains("ffxivboot"))
                {
                    BootPatching bootPatching = new (serverInfoModel);
                    bootPatching.LaunchBoot();
                }else if (args[0].Contains("ffxivlogin"))
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