using System.Diagnostics;
using System;
using System.Windows;
using Aria2Manager.ViewModels;
using System.Windows.Controls;
using Hardcodet.Wpf.TaskbarNotification;

namespace Aria2Manager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon _taskbarIcon = null!;
        private readonly Aria2ProcessManager _aria2ProcessManager = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //获取欲启动进程名，防止运行两次
            string strProcessName = Process.GetCurrentProcess().ProcessName;
            //检查进程是否已经启动，已经启动则显示报错信息退出程序。
            if (Process.GetProcessesByName(strProcessName).Length > 1)
            {
                MessageBox.Show("程序不能运行2次！", "系统错误", MessageBoxButton.OK, MessageBoxImage.Hand, MessageBoxResult.OK);
                Current.Shutdown();
                return; // 否则会继续执行
            }

            WindowBoundsSettings windowBoundsSettings = new ();

            _taskbarIcon = (TaskbarIcon)FindResource("TaskbarIcon");
            _taskbarIcon.DataContext = new TaskbarIconViewModel(
                _aria2ProcessManager,
                windowBoundsSettings);
            var contextMenu = (ContextMenu)FindResource("ContextMenu");
            contextMenu.DataContext = _taskbarIcon.DataContext;
            _taskbarIcon.ContextMenu = contextMenu;
        }

        private async void Application_Exit(object sender, ExitEventArgs e)
        {
            if (_aria2ProcessManager.IsRunning == true)
            {
                await _aria2ProcessManager.DisposeAsync();
            }
        }
    }

}
