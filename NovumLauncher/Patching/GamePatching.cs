using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Common;
using Common.Models;
using Common.StructLayout;
using Common.Utility;
using Common.Wrappers;

namespace NovumLauncher.Patching;

public class GamePatching
{
    private readonly ServerInfoModel _serverInfoModel;
    private readonly CreateProcessWrapper _createProcessWrapper;
    private readonly Common.Utility.Utils _utils;
    

    public GamePatching(IReadOnlyList<string> args ,ServerInfoModel serverInfoModel)
    {
        _utils = Common.Utility.Utils.Instance;
        _serverInfoModel = serverInfoModel;
        _createProcessWrapper = new CreateProcessWrapper($"{args[0]} {args[1]}", _utils.GameInstallLocation());
        
    }

    public void LaunchGame()
    {
        NativeMethods.DebugActiveProcessStop(_createProcessWrapper.PInfo.dwProcessId);
        
        IntPtr affinity = new(0x00000000000000FF);
        NativeMethods.SetProcessAffinityMask(_createProcessWrapper.PInfo.hProcess, affinity);
        NativeMethods.SetThreadAffinityMask(_createProcessWrapper.PInfo.hThread, affinity);

        string ipAddress = _utils.GetHostNameIP(_serverInfoModel.LobbyServerAddress);
        if (!ApplyPatches(_createProcessWrapper.PInfo.hProcess, _createProcessWrapper.PInfo.hThread, ipAddress))
        {
            throw new Exception("Error While Patching");
        }

        Console.WriteLine("Game is Patched and DLL is Injected");
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


        /*
        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, Constants.GameTimePatchOffset), Constants.TimePatch,
            Constants.TimePatch.Length);
            */


        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, Constants.GameLobbyHostNameOffset), hostnameBytes,
            hostnameBytes.Length + 1);

        _utils.InjectDllAndResumeThread(hProcess, hThread,
            $"{_utils.GameInstallLocation()}\\ApiHooks.dll");

        return true;
    }
}