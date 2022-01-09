
using System.Net;

namespace DalamudLauncher
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            /*string result = "";
            Utils utils = Utils.Instance;
            byte[] test = utils.FFXIVLoginStringEncode(0x739, "account.square-enix");
            foreach (byte b in test) result += b.ToString("x2") +" ";
            Console.WriteLine(result);*/
            string? hostName;
            string? port;
            do
            {
                Console.WriteLine("Please Enter the hostname of the Patch Server (Without the port)");
                hostName = Console.ReadLine();
                
                if (hostName != null)
                {
                    if (Uri.CheckHostName(hostName) == UriHostNameType.Unknown)
                    {
                        Console.WriteLine("Invalid Host Name");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("The Host Name Value is Empty");
                    continue;
                }
                
                Console.WriteLine("Please Enter the Port for the hostname");
                port = Console.ReadLine();
                
                if (port != null)
                {
                    int valid = int.Parse(port);

                    if (valid <= 0)
                    {
                        Console.WriteLine("Invalid Port Number");
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("The Port Number Value is Empty");
                    continue;
                }

                break;
                
            } while (true);
            
            
            BootPatching bootPatching = new(hostName!,port!);
            bootPatching.LaunchBoot();
        }
        
    }
}