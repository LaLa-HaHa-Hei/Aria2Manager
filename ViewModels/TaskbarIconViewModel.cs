using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aria2Manager.Views;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Aria2Manager.ViewModels
{
    public partial class TaskbarIconViewModel : ObservableObject
    {
        private readonly Aria2ProcessManager _aria2ProcessManager;
        private readonly WindowBoundsSettings _windowBoundsSettings;
        private MainWindow? _mainWindow = null;
        private Aria2LogWindow? _aria2LogWindow = null;

        public TaskbarIconViewModel(Aria2ProcessManager aria2ProcessManager, WindowBoundsSettings windowBoundsSettings)
        {
            _aria2ProcessManager = aria2ProcessManager;
            _windowBoundsSettings = windowBoundsSettings;
            try
            {
                _aria2ProcessManager.Start();
                ShowMainWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                ExitApplication();
            }
        }

        // Aria2进程
        [RelayCommand]
        public void StartAria2() => _aria2ProcessManager.Start();
        [RelayCommand]
        public async Task StopAria2() => await _aria2ProcessManager.StopAsync();
        [RelayCommand]
        public async Task RestartAria2()
        {
            await _aria2ProcessManager.StopAsync();
            StartAria2();
        }

        [RelayCommand]
        public void OpenAria2LogWindow()
        {
            if (_aria2LogWindow == null)
            {
                _aria2LogWindow = new(_aria2ProcessManager);
                _aria2LogWindow.Closed += (s, e) => _aria2LogWindow = null;
                _aria2LogWindow.Show();
            }
            else
            {
                _aria2LogWindow.Activate();
            }
        }

        // MainWindow
        [RelayCommand]
        public void ShowMainWindow()
        {
            if (_mainWindow == null)
            {
                _mainWindow = new(_windowBoundsSettings);
                _mainWindow.Closed += (s, e) => _mainWindow = null;
                _mainWindow.Show();
            }
            else if (_mainWindow.IsVisible == false)
                _mainWindow.Show();
            else
                _mainWindow.Activate();
        }

        [RelayCommand]
        public void HideMainWindow() => _mainWindow?.Hide();

        [RelayCommand]
        public void ToggleWindowVisibility()
        {
            if (_mainWindow?.IsVisible == true)
                HideMainWindow();
            else
                ShowMainWindow();
        }

        [RelayCommand]
        public void ExitApplication()
        {
            Application.Current.Shutdown();
        }
    }
}
