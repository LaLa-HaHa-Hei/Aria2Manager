using System.IO;
using System.Windows;

namespace Aria2Manager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly WindowBoundsSettings _windowBoundsSettings;
        private readonly string _htmlUri = null!;
        private bool _isWebViewInitialized = false;
        public MainWindow(WindowBoundsSettings windowBoundsSettings)
        {
            InitializeComponent();

            _windowBoundsSettings = windowBoundsSettings;
            string _htmlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AriaNg", "index.html");
            // 获取本地html文件路径
            if (File.Exists(_htmlFilePath))
            {
                _htmlUri = new Uri(_htmlFilePath).AbsoluteUri;
            }
            else
            {
                MessageBox.Show("AriaNg的HTML文件不存在: " + _htmlFilePath);
                throw new FileNotFoundException($"{_htmlFilePath} not found", _htmlFilePath);
            }
            WindowBounds windowBounds = _windowBoundsSettings.GetWindowBounds();
            Width = windowBounds.Width;
            Height = windowBounds.Height;
            Left = windowBounds.Left;
            Top = windowBounds.Top;
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            // 确保WebView2初始化
            await AriaNgWebView2.EnsureCoreWebView2Async();
            _isWebViewInitialized = true; // 初始化完成
            AriaNgWebView2.CoreWebView2.Navigate(_htmlUri);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _windowBoundsSettings.SaveWindowBounds(new WindowBounds
            {
                Width = Width,
                Height = Height,
                Left = Left,
                Top = Top
            });
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!_isWebViewInitialized) return; // 防止初始化没完成就执行

            if (IsVisible)
            {
                AriaNgWebView2.CoreWebView2.Navigate(_htmlUri);
            }
            else
            {
                AriaNgWebView2.CoreWebView2.NavigateToString("<html></html>");
            }
        }
    }
}