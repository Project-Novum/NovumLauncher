using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using Common.Models;
using Common.Utility;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovumLauncherUI.Patching;

namespace NovumLauncherUI.MVVM.ViewModel;

public class MainWindowViewModel : ObservableObject
{
    private ObservableCollection<ServerInfoModel> _serverList;
    private ServerInfoModel _selectedServer;
    private Utils _utils;

    public ICommand PatchBootCommand { get; set; }

    public MainWindowViewModel()
    {
        _utils = Utils.Instance;
        PatchBootCommand = new RelayCommand(PatchBootTask);

        if (!File.Exists($"{AppDomain.CurrentDomain.BaseDirectory}ServerList.json"))
        {
            string tempJson = @"
{
  ""ServerList"":[
    {
      ""ServerName"":""local"",
      ""PatchServerAddress"":""localhost"",
      ""PatchServerPort"":""54996"",
      ""LoginServerAddress"":""http://localhost:8081"",
      ""LobbyServerAddress"":""localhost""
    }
]
}";
            File.WriteAllText(tempJson, $"{AppDomain.CurrentDomain.BaseDirectory}ServerList.json");
        }

        _serverList =
            JObject.Parse(File.ReadAllText($"{AppDomain.CurrentDomain.BaseDirectory}ServerList.json"))[
                    "ServerList"]!
                .ToObject<ObservableCollection<ServerInfoModel>>()!;

        if (_serverList.Count > 0)
            _selectedServer = _serverList[0];
    }

    public ObservableCollection<ServerInfoModel> ServerList
    {
        get => _serverList;
        set => _serverList = value ?? throw new ArgumentNullException(nameof(value));
    }

    public ServerInfoModel SelectedServer
    {
        get => _selectedServer;
        set => _selectedServer = value ?? throw new ArgumentNullException(nameof(value));
    }

    private void PatchBootTask()
    {

        string gameDir = _utils.GameInstallLocation();

        File.WriteAllText($"{gameDir}\\SelectedServer.json", JsonConvert.SerializeObject(_selectedServer));
        File.Copy($"{Directory.GetCurrentDirectory()}\\ApiHooks.dll", $"{gameDir}\\ApiHooks.dll", true);
        string exePath = $"{Directory.GetCurrentDirectory()}\\NovumLauncher.exe";
        Registry.SetValue($"{Constants.ImageExecutionOptions}\\ffxivlogin.exe", "debugger", exePath);
        Registry.SetValue($"{Constants.ImageExecutionOptions}\\ffxivgame.exe", "debugger", exePath);
        BootPatching bootPatching = new(_selectedServer);
        if (bootPatching.LaunchBoot())
        {
            App.Current.Shutdown();
        }
    }
}