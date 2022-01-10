namespace Common.Models;

public class ServerInfoModel
{
    public ServerInfoModel(string serverName, string patchServerAddress, string patchServerPort, string loginServerAddress, string lobbyServerAddress)
    {
        ServerName = serverName;
        PatchServerAddress = patchServerAddress;
        PatchServerPort = patchServerPort;
        LoginServerAddress = loginServerAddress;
        LobbyServerAddress = lobbyServerAddress;
    }

    public string ServerName { get; set; }

    public string PatchServerAddress { get; set; }

    public string PatchServerPort { get; set; }

    public string LoginServerAddress { get; set; }

    public string LobbyServerAddress { get; set; }
}