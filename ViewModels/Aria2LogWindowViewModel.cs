using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Aria2Manager.ViewModels
{
    public partial class Aria2LogWindowViewModel : ObservableObject
    {
        private readonly Aria2ProcessManager _aria2ProcessManager;
        private readonly DispatcherTimer _updateLogTimer = new();
        [ObservableProperty]
        private string _outputLog = string.Empty;
        [ObservableProperty]
        private string _errorLog = string.Empty;

        public Aria2LogWindowViewModel(Aria2ProcessManager aria2ProcessManager)
        {
            _aria2ProcessManager = aria2ProcessManager;
            _updateLogTimer.Interval = TimeSpan.FromSeconds(1);
            _updateLogTimer.Tick += (s, e) =>
            {
                OutputLog = _aria2ProcessManager.GetOutputLog();
                ErrorLog = _aria2ProcessManager.GetErrorLog();
            };
            _updateLogTimer.Start();
        }
    }
}
