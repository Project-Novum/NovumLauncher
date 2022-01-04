using System.Runtime.InteropServices;

namespace Common.StructLayout;

[StructLayout(LayoutKind.Sequential)]
public struct ProcessInformation
{
    public IntPtr hProcess;
    public IntPtr hThread;
    public Int32 dwProcessId;
    public Int32 dwThreadId;
}