namespace Common.Utility;

public static class Constants
{
    
    public const string RegLocation =
        "Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F2C4E6E0-EB78-4824-A212-6DF6AF0E8E82}";

    public const string ImageExecutionOptions = 
        "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\Image File Execution Options";
    
    
    public const int GameTimePatchOffset = 0x9A15E3;
    public const int GameLobbyHostNameOffset = 0xB90110;
    public const int LoginHostNameOffset = 0x53EA0;
    
    public const string BootSha1InstallVersion = "999EFB09D7D94C9C8106A75688CF3BED7C1FBA84";
    
    public static readonly byte[] OriginalRsaSign = { 0x8B,0x44,0x24,0x24,0x83,0xC4,0x0C};
    
    public static readonly byte[] RsaFunctionPatch = { 0xB8, 0x1F, 0x00, 0x00, 0x00, 0xC3};
    public static readonly byte[] RsaPatternPatch = { 0x5E, 0x87, 0x48, 0x48, 0x06, 0xF8, 0x88 };
    public static readonly byte[] TimePatch = { 0xB8, 0x12, 0xE8, 0xE0, 0x50 };
    
    
}