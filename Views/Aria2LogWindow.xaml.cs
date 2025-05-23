using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Aria2Manager.ViewModels;

namespace Aria2Manager.Views
{
    /// <summary>
    /// Aria2LogWindow.xaml 的交互逻辑
    /// </summary>
    public partial class Aria2LogWindow : Window
    {
        public Aria2LogWindow(Aria2ProcessManager aria2ProcessManager)
        {
            InitializeComponent();
            DataContext = new Aria2LogWindowViewModel(aria2ProcessManager);
        }
    }
}
