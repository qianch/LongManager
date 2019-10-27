using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
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
using Microsoft.EntityFrameworkCore;

namespace LongManagerClient.Pages.User
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

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            UserDataGrid.ItemsSource = LongDbContext.FrameUser
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = LongDbContext.FrameUser.Count();
            Pager.InitButton();
            UserDataGrid.ItemsSource = LongDbContext.FrameUser
                .Take(Pager.LongPage.PageSize)
                .ToList();
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
            Search();
        }

        protected override void Search() 
        {
            var frameUsers = LongDbContext.FrameUser.AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(TxtUserName.Text))
            {
                frameUsers = frameUsers.Where(x => x.UserName.Contains(TxtUserName.Text));
            }

            Pager.LongPage.AllCount = frameUsers.Count();
            Pager.LongPage.Search = TxtUserName.Text;
            Pager.InitButton();

            UserDataGrid.ItemsSource = frameUsers
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }
    }
}
