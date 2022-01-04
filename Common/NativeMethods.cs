using System.Runtime.InteropServices;
using System.Text;
using Common.StructLayout;

namespace Common;

public class NativeMethods
{
    [DllImport("kernel32.dll", CharSet=CharSet.Unicode,SetLastError = true)]
    public static extern bool CreateProcess
    (
        string lpApplicationName,
        string lpCommandLine,
        SecurityAttributes lpProcessAttributes, 
        SecurityAttributes lpThreadAttributes,
        bool bInheritHandles, 
        CreateProcessFlags dwCreationFlags,
        IntPtr lpEnvironment,
        string lpCurrentDirectory,
        ref StartupInfo lpStartupInfo,
        out ProcessInformation lpProcessInformation
    );
    

    
    [DllImport("kernel32.dll")]
    public static extern uint ResumeThread(IntPtr hThread);
    
    [DllImport("kernel32.dll", SetLastError=true)]
    public static extern bool CloseHandle(IntPtr handle);
    
    
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool GetThreadContext(IntPtr hThread, ref CONTEXT lpContext);
    
    [DllImport("kernel32", CharSet=CharSet.Ansi, ExactSpelling=true, SetLastError=true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);
    
    [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string lpModuleName);
    
    [DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress,
        uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);
    
    
    [DllImport("kernel32.dll", SetLastError = true)]
   public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        out IntPtr lpBuffer,
        int dwSize,
        out IntPtr lpNumberOfBytesRead);
   
   
   [DllImport("kernel32.dll")]
   public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize,
       out IntPtr lpNumberOfBytesRead);
  
    
    
    [DllImport("kernel32.dll")]
    public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress,
        UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);
    
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        Int32 nSize,
        out IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll")]
    public static extern IntPtr CreateRemoteThread(IntPtr hProcess,
        IntPtr lpThreadAttributes, uint dwStackSize, IntPtr lpStartAddress,
        IntPtr lpParameter, uint dwCreationFlags, out IntPtr lpThreadId);
    
    [DllImport("kernel32.dll", SetLastError=true)]
    public static extern UInt32 WaitForSingleObject(IntPtr hHandle, UInt32 dwMilliseconds);
    
    [DllImport("kernel32.dll", SetLastError=true, ExactSpelling=true)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress,
        int dwSize, FreeType dwFreeType);
    
    [DllImport("kernel32.dll")]
    public static extern bool SetProcessAffinityMask(IntPtr hProcess,
        IntPtr dwProcessAffinityMask);
    [DllImport("kernel32.dll")]
    public static extern UIntPtr SetThreadAffinityMask(IntPtr hThread,
        IntPtr dwThreadAffinityMask);
    
    [DllImport("kernel32.dll")]
    public static extern uint GetTickCount();
    
    [DllImport("msvcrt.dll", CharSet = CharSet.Unicode,
        CallingConvention = CallingConvention.Cdecl)]
    public static extern int sprintf(
        StringBuilder buffer,
        string format,
        __arglist);
    
    [DllImport("kernel32.dll")]
    public static extern void GetSystemInfo(out SystemInfo lpSystemInfo);

    [DllImport("kernel32.dll", SetLastError=true)]
    public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemoryBasicInformation lpBuffer, uint dwLength);
    
    [DllImport("msvcrt.dll", CallingConvention=CallingConvention.Cdecl)]
    public static extern int memcmp(byte[] b1, byte[] b2, long count);

}