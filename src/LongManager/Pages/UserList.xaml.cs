using LongManager.Core;
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
    public partial class UserList : Page
    {
        private LongDbContext _longDBContext = new LongDbContext();
        public UserList()
        {
            InitializeComponent();
        }

        private void UserList_Loaded(object sender, RoutedEventArgs e)
        {
            UserDataGrid.ItemsSource = _longDBContext.FrameUsers.ToList();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var pageFrame = GlobalCache.Instance.Frame;
            var userEdit = GlobalCache.Instance.AllPages["UserEdit"] as UserEdit;
            userEdit.Key = editButton.Tag as string;

            var window = new NavigationWindow
            {
                ShowsNavigationUI = false,
                Width = 800,
                Height = 450,
                ShowInTaskbar = false,
                ResizeMode = ResizeMode.NoResize
            };
            window.NavigationService.Navigate(userEdit);
            window.ShowDialog();
        }
    }
}
