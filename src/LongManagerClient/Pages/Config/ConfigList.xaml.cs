using Microsoft.EntityFrameworkCore;
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

namespace LongManagerClient.Pages.Config
{
    /// <summary>
    /// ConfigList.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigList : BasePage
    {
        public ConfigList()
        {
            InitializeComponent();
            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            ConfigDataGrid.ItemsSource = LongDbContext.FrameConfig
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = LongDbContext.FrameConfig.Count();
            Pager.InitButton();
            ConfigDataGrid.ItemsSource = LongDbContext.FrameConfig
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var window = new ConfigEdit
            {
                ExtraData = editButton.Tag,
            };
            window.CallBack = Search;
            window.ShowDialog();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        protected void Search()
        {
            var configs = LongDbContext.FrameConfig.AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(TxtConfigName.Text))
            {
                configs = configs.Where(x => x.ConfigName.Contains(TxtConfigName.Text));
            }

            Pager.LongPage.AllCount = configs.Count();
            Pager.LongPage.Search = TxtConfigName.Text;
            Pager.InitButton();

            ConfigDataGrid.ItemsSource = configs
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }
    }
}
