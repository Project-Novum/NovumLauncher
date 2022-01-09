using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Common.StructLayout;

namespace Common.Wrappers;

public class CreateProcessWrapper
{
    private ProcessInformation _pInfo;
    bool _isProcessCreated = false;

    public CreateProcessWrapper(string command,string workingDirectory)
    {
        StartupInfo startupInfo = new StartupInfo();
        _pInfo = new ProcessInformation();
        CreateProcessFlags createProcessFlags =
            CreateProcessFlags.CREATE_SUSPENDED | CreateProcessFlags.NORMAL_PRIORITY_CLASS | CreateProcessFlags.DEBUG_ONLY_THIS_PROCESS;
        
        _isProcessCreated = NativeMethods.CreateProcess(null!, command, null!, null!,
            false, createProcessFlags,
            IntPtr.Zero, workingDirectory, ref startupInfo, out _pInfo);

        if (!_isProcessCreated)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public CreateProcessWrapper(string application)
    {
        StartupInfo startupInfo = new StartupInfo();
        _pInfo = new ProcessInformation();
        CreateProcessFlags createProcessFlags =
            CreateProcessFlags.CREATE_SUSPENDED | CreateProcessFlags.NORMAL_PRIORITY_CLASS;
        
        _isProcessCreated = NativeMethods.CreateProcess(application, null!, null!, null!,
            false, createProcessFlags,
            IntPtr.Zero, null!, ref startupInfo, out _pInfo);

        if (!_isProcessCreated)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    public ProcessInformation PInfo
    {
        get => _pInfo;
        set => _pInfo = value;
    }

    public bool IsProcessCreated
    {
        get => _isProcessCreated;
        set => _isProcessCreated = value;
    }
}