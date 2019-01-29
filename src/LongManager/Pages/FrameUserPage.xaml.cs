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

namespace LongManager.Pages
{
    /// <summary>
    /// FrameUserPage.xaml 的交互逻辑
    /// </summary>
    public partial class FrameUserPage : Page
    {
        private LongDbContext _longDBContext = new LongDbContext();
        public FrameUserPage()
        {
            InitializeComponent();
        }

        private void User_Loaded(object sender, RoutedEventArgs e)
        {
            UserDataGrid.ItemsSource = _longDBContext.FrameUsers.ToList();
        }
    }
}
