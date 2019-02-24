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

namespace LongManager.Pages.User
{
    /// <summary>
    /// FrameUserPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserList : BasePage
    {
        public UserList()
        {
            InitializeComponent();
            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void Pager_PageIndexChange(int pageIndex, EventArgs e)
        {
            UserDataGrid.ItemsSource = _longDBContext.FrameUsers.ToList().Skip(Pager.LongPage.PageSize * (pageIndex - 1)).Take(Pager.LongPage.PageSize);
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = _longDBContext.FrameUsers.Count();
            Pager.InitButton();
            UserDataGrid.ItemsSource = _longDBContext.FrameUsers.ToList().Take(Pager.LongPage.PageSize);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var window = new UserEdit
            {
                ExtraData = editButton.Tag,
            };
            window.ShowDialog();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtUserName.Text))
            {
                UserDataGrid.ItemsSource = _longDBContext.FrameUsers
                    .Where(x => x.UserName.Contains(TxtUserName.Text))
                    .ToList();
            }
            else
            {
                UserDataGrid.ItemsSource = _longDBContext.FrameUsers.ToList();
            }
        }
    }
}
