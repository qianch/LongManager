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

namespace LongManagerClient.Pages.JiangSuOut
{
    /// <summary>
    /// JiangSuOutList.xaml 的交互逻辑
    /// </summary>
    public partial class JiangSuOutList : BasePage
    {
        public JiangSuOutList()
        {
            InitializeComponent();

            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = _longDBContext.OutInfo.Where(x => x.CountryPosition == "38").Count();
            Pager.InitButton();
            MailDataGrid.ItemsSource = _longDBContext.OutInfo
                .Where(x => x.CountryPosition == "38")
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            MailDataGrid.ItemsSource = _longDBContext.OutInfo
                .Where(x => x.CountryPosition == "38")
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var mails = _longDBContext.OutInfo.Where(x => x.CountryPosition == "38").AsEnumerable();
            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                mails = mails.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            Pager.LongPage.AllCount = mails.Count();
            Pager.LongPage.Search = TxtMailNO.Text;
            Pager.InitButton();

            MailDataGrid.ItemsSource = mails
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void PositionBtn_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
