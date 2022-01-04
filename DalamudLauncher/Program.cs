
using System.Net;

namespace DalamudLauncher
{
    public static class Program
    {
        public static void Main(string[] args)
        {
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
                    }
                }
                else
                {
                    Console.WriteLine("The Host Name Value is Empty");

                }
                
                Console.WriteLine("Please Enter the Port for the hostname");
                port = Console.ReadLine();
                
                if (port != null)
                {
                    int valid = Int32.Parse(port);

                    if (valid <= 0)
                    {
                        Console.WriteLine("Invalid Port Number");
                    }
                }
                else
                {
                    Console.WriteLine("The Port Number Value is Empty");
                }

                break;
                
            } while (true);
            
            
            BootPatching bootPatching = new(hostName!,port!);
            bootPatching.LaunchBoot();
        }
        
    }
}