using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Common.Wrappers;

public static class MemoryAccessWrapper
{
    public static void ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, out IntPtr lpBuffer, int dwSize,
        out IntPtr lpNumberOfBytesRead)
    {
        if (!NativeMethods.ReadProcessMemory(hProcess, lpBaseAddress, out lpBuffer, dwSize,
                out lpNumberOfBytesRead))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    public static void ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize,
        out IntPtr lpNumberOfBytesRead)
    {
        if (!NativeMethods.ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, dwSize,
                out lpNumberOfBytesRead))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }

    public static void VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect,
        out uint lpflOldProtect)
    {
        if (!NativeMethods.VirtualProtectEx(hProcess, lpAddress, dwSize, flNewProtect,
                out lpflOldProtect))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }


    public static void WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int nSize,
        out IntPtr lpNumberOfBytesWritten)
    {
        if (!NativeMethods.WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, nSize, out lpNumberOfBytesWritten))
            throw new Win32Exception(Marshal.GetLastWin32Error());
    }
}