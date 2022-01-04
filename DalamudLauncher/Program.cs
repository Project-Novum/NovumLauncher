
namespace DalamudLauncher
{
    public static class Program
    {
        private const string Url = "http://127.0.0.1/login.php";
        

        public static void Main(string[] args)
        {
            BootPatching bootPatching = new("localhost");
            bootPatching.LaunchBoot();
        }
        
    }
}