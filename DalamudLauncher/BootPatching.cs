using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Common;
using Common.StructLayout;
using Common.Wrappers;

namespace DalamudLauncher;

public class BootPatching
{
    private readonly string _hostName;
    private readonly Utils _utils;

    public BootPatching(string hostName)
    {
        _hostName = hostName;
        _utils = Utils.Instance;
    }

    public void LaunchBoot()
    {
        string? workingDirectory = _utils.GameInstallLocation();
        string ipAddress = _utils.GetHostNameIP(_hostName);

        CreateProcessWrapper createProcessWrapper = new($"{workingDirectory}\\ffxivboot.exe");
        IntPtr affinity = new(0x00000000000000FF);
        NativeMethods.SetProcessAffinityMask(createProcessWrapper.PInfo.hProcess, affinity);
        NativeMethods.SetThreadAffinityMask(createProcessWrapper.PInfo.hThread, affinity);

        if (!ApplyPatches(createProcessWrapper.PInfo.hProcess, createProcessWrapper.PInfo.hThread, ipAddress))
        {
            throw new Exception("Error while patching");
        }
    }

    private bool ApplyPatches(IntPtr hProcess, IntPtr hThread, string lobbyHostName)
    {
        CONTEXT threadContext = new()
        {
            ContextFlags = (uint)CONTEXT_FLAGS.CONTEXT_FULL
        };

        int rsaFunctionOffset = 0;
        int rsaPatternOffset = 0;
        int lobbyOffset = 0;
        int hostNameOffset = 0;


        if (!NativeMethods.GetThreadContext(hThread, ref threadContext))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        IntPtr imageBaseAddressPtr = new(threadContext.Ebx + 8);

        MemoryAccessWrapper.ReadProcessMemory(hProcess, imageBaseAddressPtr, out IntPtr imageBaseAddress, 4,
            out IntPtr _);

        
        if (CheckBootOffset(hProcess, imageBaseAddress))
        {
            rsaFunctionOffset = Constants.BootRsaFunctionOffsetUpdatedVersion;
            rsaPatternOffset = Constants.BootRsaPatternOffsetUpdatedVersion;
            lobbyOffset = Constants.BootLobbyOffsetUpdatedVersion;
            hostNameOffset = Constants.BootHostNameOffsetUpdatedVersion;
        }
        else
        {
            rsaFunctionOffset = Constants.BootRsaFunctionOffsetInstallVersion;
            rsaPatternOffset = Constants.BootRsaPatternOffsetInstallVersion;
            lobbyOffset = Constants.BootLobbyOffsetInstallVersion;
            hostNameOffset = Constants.BootHostNameOffsetInstallVersion;
        }

        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, rsaFunctionOffset), Constants.RsaFunctionPatch,
            Constants.RsaFunctionPatch.Length);

        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, rsaPatternOffset), Constants.RsaPatternPatch,
            Constants.RsaPatternPatch.Length);
        
        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, lobbyOffset), Constants.BootLobbyPatch,
            Constants.BootLobbyPatch.Length);

        byte[] hostnameBytes = Encoding.Default.GetBytes(lobbyHostName);
        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, hostNameOffset), hostnameBytes,
            hostnameBytes.Length + 1);


        _utils.InjectDllAndResumeThread(hProcess, hThread,
            $"{Directory.GetCurrentDirectory()}\\AffinityInjector.dll");

        
        

        return true;
    }

    private bool CheckBootOffset(IntPtr hProcess, IntPtr address)
    {
        byte[] buffer = new byte[7];

        MemoryAccessWrapper.ReadProcessMemory(hProcess, IntPtr.Add(address, 0x646EC), buffer, 7,
            out IntPtr _);

        if (buffer.Length == Constants.OriginalRsaSign.Length &&
            NativeMethods.memcmp(buffer, Constants.OriginalRsaSign, buffer.Length) == 0)
        {
            return true;
        }

        return false;
    }
}