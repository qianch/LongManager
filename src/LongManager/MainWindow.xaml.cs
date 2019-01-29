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

namespace LongManager
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ILog log = LogManager.GetLogger(typeof(MainWindow));
        public MainWindow()
        {
            InitializeComponent();

            //加载所有的Page
            GlobalCache.Instance.AllPages.Add("UserList", new UserList());
            GlobalCache.Instance.AllPages.Add("UserEdit", new UserEdit());
            GlobalCache.Instance.Frame = PageFrame;
        }

        private void FrameUserBtn_Click(object sender, RoutedEventArgs e)
        {
            GlobalCache.Instance.Frame.NavigationService.Navigate(GlobalCache.Instance.AllPages["UserList"]);
        }
    }
}
