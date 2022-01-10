namespace Common.Offsets;

public class BootInstalledVersionOffset : IBootOffSet
{
    public int GetRsaFunctionOffSet()
    {
        return 0x5DF50;
    }

    public int GetRsaPatternOffset()
    {
        return 0x5e32C;
    }

    public int GetLobbyOffset()
    {
        return 0x8E5C6C;
    }

    public int GetHostNamePortOffset()
    {
        return 0x8E62D4;
    }

    public int GetHostNameOffset()
    {
        return 0x8E62DC;
    }

    public int GetSecureSquareEnixOffset()
    {
        return 0x90A4A0;
    }
}