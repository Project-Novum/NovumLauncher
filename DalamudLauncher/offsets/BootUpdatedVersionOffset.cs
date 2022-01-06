namespace DalamudLauncher.offsets;

public class BootUpdatedVersionOffset : IBootOffSet
{
    public int GetRsaFunctionOffSet()
    {
        return 0x64310;
    }

    public int GetRsaPatternOffset()
    {
        return 0x646EC;
    }

    public int GetLobbyOffset()
    {
        return 0x965D08;
    }

    public int GetHostNamePortOffset()
    {
        return 0x9663FC;
    }

    public int GetHostNameOffset()
    {
        return 0x966404;
    }

    public int GetSecureSquareEnixOffset()
    {
        return 0x99212C;
    }
}