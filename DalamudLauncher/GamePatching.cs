using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Common;
using Common.StructLayout;
using Common.Wrappers;

namespace DalamudLauncher;

public class GamePatching
{
    private readonly string _username;
    private readonly string _password;
    private readonly string _hostName;
    private readonly string _loginUrl;
    private readonly Utils _utils;

    public GamePatching(string username, string password, string hostName, string loginUrl)
    {
        _username = username;
        _password = password;
        _hostName = hostName;
        _loginUrl = loginUrl;
        _utils = Utils.Instance;
    }

    public void LaunchGame()
    {
        string? workingDirectory = _utils.GameInstallLocation();
        string? sessionId = Login();
        if (sessionId == null || workingDirectory == null) return;

        string command = GetCommandLine(sessionId);

        CreateProcessWrapper processWrapper = new(command, workingDirectory);
        IntPtr affinity = new(0x00000000000000FF);
        NativeMethods.SetProcessAffinityMask(processWrapper.PInfo.hProcess, affinity);
        NativeMethods.SetThreadAffinityMask(processWrapper.PInfo.hThread, affinity);

        string ipAddress = _utils.GetHostNameIP(_hostName);
        if (!ApplyPatches(processWrapper.PInfo.hProcess, processWrapper.PInfo.hThread, ipAddress))
        {
            throw new Exception("Error While Patching");
        }

        Console.WriteLine("Game is Patched and DLL is Injected");
    }


    private string? Login()
    {
        using HttpClient client = new HttpClient();
        StringContent data = new(
            $"username={_username}&password={_password}&login=login",
            Encoding.UTF8,
            "application/x-www-form-urlencoded");

        HttpResponseMessage response = client.PostAsync(_loginUrl, data).Result;

        using StreamReader streamReader = new(response.Content.ReadAsStream());
        string result = streamReader.ReadToEnd();

        Match match = new Regex(Constants.RegexPattern).Match(result);

        if (!match.Success) return null;

        string ffxivUri = match.Groups[1].ToString();
        Console.WriteLine(ffxivUri);
        string? sessionId = HttpUtility.ParseQueryString(new Uri(ffxivUri).Query).Get("sessionId");

        return sessionId;
    }


    private string GetCommandLine(string sessionId)
    {
        UInt32 tickCount = NativeMethods.GetTickCount();
        string commandLine =
            $" T ={tickCount} /LANG =en-us /REGION =2 /SERVER_UTC =1356916742 /SESSION_ID ={sessionId}";

        string seed = (tickCount & ~0xFFFF).ToString("x8");

        byte[] encryptionKey = Encoding.Default.GetBytes(seed);
        Blowfish blowfish = new Blowfish(encryptionKey);

        byte[] data = new byte[1024]; //Encoding.Default.GetBytes(commandLine);

        Encoding.Default.GetBytes(commandLine, 0, commandLine.Length, data, 0);
        int dataLength = commandLine.Length + 1;
        blowfish.Encipher(data, 0, (dataLength & ~0x7));

        string base64String = Convert.ToBase64String(data, 0, dataLength);
        base64String = base64String.Replace('+', '-');
        base64String = base64String.Replace('/', '_');

        return $"\\ffxivgame.exe sqex0002{base64String}!////";
    }

    private bool ApplyPatches(IntPtr hProcess, IntPtr hThread, string lobbyHostName)
    {
        CONTEXT threadContext = new()
        {
            ContextFlags = (uint)CONTEXT_FLAGS.CONTEXT_FULL
        };

        byte[] hostnameBytes = Encoding.Default.GetBytes(lobbyHostName);

        if (!NativeMethods.GetThreadContext(hThread, ref threadContext))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        IntPtr imageBaseAddressPtr = new(threadContext.Ebx + 8);

        MemoryAccessWrapper.ReadProcessMemory(hProcess, imageBaseAddressPtr, out IntPtr imageBaseAddress, 4,
            out IntPtr _);


        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, Constants.GameTimePatchOffset), Constants.TimePatch,
            Constants.TimePatch.Length);


        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, Constants.GameLobbyHostNameOffset), hostnameBytes,
            hostnameBytes.Length + 1);

        _utils.InjectDllAndResumeThread(hProcess, hThread,
            $"{Directory.GetCurrentDirectory()}\\AffinityInjector.dll");

        return true;
    }
}