using log4net;
using LongManager.Core;
using LongManager.Core.DataBase;
using LongManager.Pages;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LongManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : BaseWindow
    {
        private ILog log = LogManager.GetLogger(typeof(MainWindow));
        private DispatcherTimer _showTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            //加载所有的Page
            GlobalCache.Instance.AllPages.Add("Welcome", new Welcome());
            GlobalCache.Instance.AllPages.Add("UserList", new UserList());
            GlobalCache.Instance.AllPages.Add("UserEdit", new UserEdit());
            GlobalCache.Instance.AllPages.Add("LogList", new LogList());
            GlobalCache.Instance.AllPages.Add("CarList", new CarList());
            GlobalCache.Instance.AllPages.Add("SystemConfig", new SystemConfig());
            GlobalCache.Instance.Frame = PageFrame;

            //默认加载欢迎页面
            PageFrame.NavigationService.Navigate(GlobalCache.Instance.AllPages["Welcome"]);

            //添加timer
            _showTimer.Tick += new EventHandler(GetTimer);
            _showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            _showTimer.Start();
        }

        private void FrameUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages["UserList"]);
        }

        private void CarBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages["CarList"]);
        }

        private void LogBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages["LogList"]);
        }

        private void SystemBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages["SystemConfig"]);
        }

        public void GetTimer(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            TxtDate.Text = date.ToString("dddd", new System.Globalization.CultureInfo("zh-cn")) + date.ToString("yyyy年MM月dd日 HH:mm:ss");
        }
    }
}
