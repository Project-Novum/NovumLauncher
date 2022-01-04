using System.Runtime.InteropServices;

namespace Common.StructLayout;

[StructLayout(LayoutKind.Sequential)]
public struct MemoryBasicInformation
{
    public int BaseAddress;
    public int AllocationBase;
    public int AllocationProtect;
    public int RegionSize;
    public int State;
    public int Protect;
    public int lType;
}