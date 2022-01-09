using System.ComponentModel;
using System.Runtime.InteropServices;
using Common;
using Common.Models;
using Common.StructLayout;
using Common.Utility;
using Common.Wrappers;

namespace NovumLauncher.Patching;

public class LoginPatching
{
    private readonly ServerInfoModel _serverInfoModel;
    private readonly CreateProcessWrapper _createProcessWrapper;
    private readonly Common.Utility.Utils _utils;

    public LoginPatching(IReadOnlyList<string> args, ServerInfoModel serverInfoModel)
    {
        _utils = Common.Utility.Utils.Instance;
        _serverInfoModel = serverInfoModel;
        _createProcessWrapper = new CreateProcessWrapper($"{args[0]} {args[1]}", _utils.GameInstallLocation());
        
        
    }

    public void ApplyPatches()
    {
        IntPtr hProcess = _createProcessWrapper.PInfo.hProcess;
        IntPtr hThread = _createProcessWrapper.PInfo.hThread;
        
        CONTEXT threadContext = new()
        {
            ContextFlags = (uint)CONTEXT_FLAGS.CONTEXT_FULL
        };
        

        if (!NativeMethods.GetThreadContext(hThread, ref threadContext))
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }

        IntPtr imageBaseAddressPtr = new(threadContext.Ebx + 8);

        MemoryAccessWrapper.ReadProcessMemory(hProcess, imageBaseAddressPtr, out IntPtr imageBaseAddress, 4,
            out IntPtr _);

        byte[] encodedUrl = _utils.FFXIVLoginStringEncode(0x739, _serverInfoModel.LoginServerAddress + char.MinValue);
        _utils.WriteToMemory(hProcess, IntPtr.Add(imageBaseAddress, Constants.LoginHostNameOffset), encodedUrl,
            encodedUrl.Length);
        
        
        NativeMethods.DebugActiveProcessStop(_createProcessWrapper.PInfo.dwProcessId);
        NativeMethods.ResumeThread(_createProcessWrapper.PInfo.hThread);
        NativeMethods.CloseHandle(_createProcessWrapper.PInfo.hProcess);
        NativeMethods.CloseHandle(_createProcessWrapper.PInfo.hThread);
    }
}