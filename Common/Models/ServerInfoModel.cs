namespace Common.Models;

public class ServerInfoModel
{
    private string _serverName;
    private string _patchServerAddress;
    private string _patchServerPort;
    private string _loginServerAddress;
    private string _lobbyServerAddress;
    
    
    public ServerInfoModel()
    {
    }

    public ServerInfoModel(string serverName, string patchServerAddress, string patchServerPort, string loginServerAddress, string lobbyServerAddress)
    {
        _serverName = serverName;
        _patchServerAddress = patchServerAddress;
        _patchServerPort = patchServerPort;
        _loginServerAddress = loginServerAddress;
        _lobbyServerAddress = lobbyServerAddress;
    }

    public string ServerName
    {
        get => _serverName;
        set => _serverName = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PatchServerAddress
    {
        get => _patchServerAddress;
        set => _patchServerAddress = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string PatchServerPort
    {
        get => _patchServerPort;
        set => _patchServerPort = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LoginServerAddress
    {
        get => _loginServerAddress;
        set => _loginServerAddress = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string LobbyServerAddress
    {
        get => _lobbyServerAddress;
        set => _lobbyServerAddress = value ?? throw new ArgumentNullException(nameof(value));
    }

    /*public string LobbyServerPort
    {
        get => _lobbyServerPort;
        set => _lobbyServerPort = value ?? throw new ArgumentNullException(nameof(value));
    }*/
}