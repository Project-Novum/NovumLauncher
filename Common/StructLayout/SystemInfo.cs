using System.Runtime.InteropServices;

namespace Common.StructLayout;

[StructLayout(LayoutKind.Sequential)]
public struct SystemInfo
{
    public ushort processorArchitecture;
    ushort reserved;
    public uint pageSize;
    public IntPtr minimumApplicationAddress;
    public IntPtr maximumApplicationAddress;
    public IntPtr activeProcessorMask;
    public uint numberOfProcessors;
    public uint processorType;
    public uint allocationGranularity;
    public ushort processorLevel;
    public ushort processorRevision;
}