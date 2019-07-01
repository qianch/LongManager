using Autofac;
using log4net;
using LongManagerClient.Command;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Pages;
using LongManagerClient.Pages.BLS;
using LongManagerClient.Pages.Car;
using LongManagerClient.Pages.City;
using LongManagerClient.Pages.In;
using LongManagerClient.Pages.Index;
using LongManagerClient.Pages.JiangSuOut;
using LongManagerClient.Pages.Label;
using LongManagerClient.Pages.Out;
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
        private DispatcherTimer _showTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            //最大化
            ResizeMode = ResizeMode.CanResize;
            ShowInTaskbar = true;
            WindowState = WindowState.Maximized;

            //绑定自定义命令
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.OpenPageCommand, OpenPageCommandBinding));
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.Exit, Exit));
            CommandBindings.Add(new CommandBinding(LongManagerClientCommands.MenuCommand, MenuCommandBinding));

            //默认加载欢迎页面
            App.Frame = PageFrame;
            PageFrame.NavigationService.Navigate(_container.ResolveNamed<BasePage>("Index"));

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
                SystemPanel.DataContext = _container.ResolveNamed<FrameUser>("CurrentUser");
                Opacity = 1;
            }
        }

        //button 关联 open page command
        private void OpenPageCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            App.Frame.NavigationService.Navigate(_container.ResolveNamed<BasePage>(param));
        }

        private void MenuCommandBinding(object sender, ExecutedRoutedEventArgs e)
        {
            var param = e.Parameter.ToString();
            switch (param)
            {
                case "About":
                    MessageBox.Show("翔龙物流科技管理系统 1.0，技术支持：qianchenchn@foxmail.com", "版权信息", MessageBoxButton.OK, MessageBoxImage.Information);
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
