using System.Runtime.InteropServices;

namespace Common.StructLayout;

[StructLayout(LayoutKind.Sequential)]
public class SecurityAttributes
{
    public Int32 Length = 0;
    public IntPtr lpSecurityDescriptor = IntPtr.Zero;
    public bool bInheritHandle = false;

    public SecurityAttributes()
    {
        this.Length = Marshal.SizeOf(this); 
    }
}