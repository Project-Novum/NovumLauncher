using System;
using System.Windows;
using System.Windows.Input;
using Common.Utility;
using Microsoft.Win32;

namespace NovumLauncherUI.MVVM.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void MainWindow_OnMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void Btn_Minimize_Click(object sender, RoutedEventArgs e)
        {
            base.WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            string fullRegLocationPath = "";

            if (Environment.Is64BitOperatingSystem)
            {
                fullRegLocationPath = $"HKEY_LOCAL_MACHINE\\SOFTWARE\\WOW6432Node\\{Constants.RegLocation}";
            }
            else
            {
                fullRegLocationPath = $"HKEY_LOCAL_MACHINE\\SOFTWARE\\{Constants.RegLocation}";
            }

            string? installLocation = Registry.GetValue(fullRegLocationPath, "InstallLocation", null) as string;
            string? displayName = Registry.GetValue(fullRegLocationPath, "DisplayName", null) as string;

            if (installLocation == null || displayName == null)
            {
                versionLabel.Content = "Currently installed: False";
            }
            else
            {
                versionLabel.Content = "Currently installed: True";
                progressBar.Value = 100;
            }

            lbl_installedVersion.Content = "2022.01.10";
        }
    }
}