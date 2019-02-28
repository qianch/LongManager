using log4net;
using LongManager.Core;
using LongManager.Core.DataBase;
using LongManagerClient.Command;
using LongManagerClient.Pages;
using LongManagerClient.Pages.Car;
using LongManagerClient.Pages.Config;
using LongManagerClient.Pages.EMS;
using LongManagerClient.Pages.Index;
using LongManagerClient.Pages.Label;
using LongManagerClient.Pages.Log;
using LongManagerClient.Pages.User;
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

namespace LongManagerClient
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

            //最大化
            ResizeMode = ResizeMode.CanResize;
            ShowInTaskbar = true;
            WindowState = WindowState.Maximized;

            //加载所有的Page
            GlobalCache.Instance.AllPages.Add("Index", new Index());
            GlobalCache.Instance.AllPages.Add("Welcome", new Welcome());
            GlobalCache.Instance.AllPages.Add("UserList", new UserList());
            GlobalCache.Instance.AllPages.Add("LogList", new LogList());
            GlobalCache.Instance.AllPages.Add("CarList", new CarList());
            GlobalCache.Instance.AllPages.Add("LabelList", new LabelList());
            GlobalCache.Instance.AllPages.Add("EMSList", new EMSList());
            GlobalCache.Instance.AllPages.Add("EMSSearch", new EMSSearch());
            GlobalCache.Instance.AllPages.Add("SystemConfig", new SystemConfig());
            GlobalCache.Instance.Frame = PageFrame;

            //绑定自定义命令
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.OpenPageCommand, OpenPageCommandBinding));
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.Exit, Exit));
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.MenuCommand, MenuCommandBinding));

            //默认加载欢迎页面
            PageFrame.NavigationService.Navigate(GlobalCache.Instance.AllPages["Index"]);

            //添加timer
            _showTimer.Tick += new EventHandler(GetTimer);
            _showTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            _showTimer.Start();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //加载登录页面
            var login = new Login
            {
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            //设置MainWindow透明度模拟蒙版效果
            Opacity = 0.3;
            if (login.ShowDialog() == false)
            {
                Application.Current.Shutdown();
            }
            else
            {
                //登录成功恢复透明度
                SystemPanel.DataContext = GlobalCache.Instance.FrameUser;
                Opacity = 1;
            }
        }

        //button 关联 open page command
        private void OpenPageCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages[param]);
        }

        private void MenuCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case "About":
                    MessageBox.Show("龙翔物流管理系统 1.0", "版权信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    break;
                default:
                    break;
            }
        }

        private void Exit(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        public void GetTimer(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            TxtDate.Text = date.ToString("yyyy年MM月dd日 HH:mm:ss  ") + date.ToString("dddd", new System.Globalization.CultureInfo("zh-cn"));
        }
    }
}
