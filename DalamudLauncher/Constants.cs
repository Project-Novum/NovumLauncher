namespace DalamudLauncher;

public static class Constants
{
    
    public const string RegLocation =
        "Microsoft\\Windows\\CurrentVersion\\Uninstall\\{F2C4E6E0-EB78-4824-A212-6DF6AF0E8E82}";

    public const string RegexPattern = "<script>window.location=\"(.*)\";</script>";

    public const int GameTimePatchOffset = 0x9A15E3;
    public const int GameLobbyHostNameOffset = 0xB90110;

    public const int BootRsaFunctionOffsetInstallVersion = 0x5DF50;
    public const int BootRsaPatternOffsetInstallVersion = 0x5e32C ;
    public const int BootLobbyOffsetInstallVersion = 0x8E5C6C;
    public const int BootHostNameOffsetInstallVersion = 0x8E62DC ;
        
    public const int BootRsaFunctionOffsetUpdatedVersion = 0x64310;
    public const int BootRsaPatternOffsetUpdatedVersion = 0x646EC;
    public const int BootLobbyOffsetUpdatedVersion = 0x965d08;
    public const int BootHostNameOffsetUpdatedVersion = 0x966404;
    
        
    public static readonly byte[] TimePatch = { 0xB8, 0x12, 0xE8, 0xE0, 0x50 };
    public static readonly byte[] OriginalRsaSign = { 0x8B,0x44,0x24,0x24,0x83,0xC4,0x0C};
    public static readonly byte[] RsaFunctionPatch = { 0xB8, 0x1F, 0x00, 0x00, 0x00, 0x0D};
    public static readonly byte[] RsaPatternPatch = { 0x5E, 0x87, 0x48, 0x48, 0x06, 0xF8, 0x88 };

    public static readonly byte[] BootLobbyPatch =
        { 0x6C,0x6F,0x61,0x63,0x86,0xF8,0xF3,0xE9,0x58,0xCC,0xF0,0xD2,0xEC,0x5C,0xC6,0xDE,0xDA };
}