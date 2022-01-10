namespace Common.Offsets;

public interface IBootOffSet
{

    public int GetRsaFunctionOffSet();
    public int GetRsaPatternOffset();
    public int GetLobbyOffset();
    public int GetHostNamePortOffset();
    public int GetHostNameOffset();
    public int GetSecureSquareEnixOffset();
}