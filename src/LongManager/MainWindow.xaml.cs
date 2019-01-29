using log4net;
using LongManager.Core.DataBase;
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
        private Dictionary<string, Uri> allPages = new Dictionary<string, Uri>();
        public MainWindow()
        {
            InitializeComponent();

            //加载所有的Page
            allPages.Add("FrameUserPage", new Uri("Pages/FrameUserPage.xaml", UriKind.Relative));
        }

        private void FrameUserBtn_Click(object sender, RoutedEventArgs e)
        {
            PageFrame.NavigationService.Navigate(allPages["FrameUserPage"]);
        }
    }
}
