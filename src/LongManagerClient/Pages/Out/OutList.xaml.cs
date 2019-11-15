using Autofac;
using LongManagerClient.Controls;
using LongManagerClient.Core;
using LongManagerClient.Core.BSL;
using LongManagerClient.Core.ClientDataBase;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LongManagerClient.Pages.Out
{
    /// <summary>
    /// EMSList.xaml 的交互逻辑
    /// </summary>
    public partial class OutList : BasePage
    {
        public OutList()
        {
            InitializeComponent();

            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            Search();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void Search()
        {
            var outInfos = LongDbContext.OutInfo.AsNoTracking().AsEnumerable<BaseOut>();
            //var history = LongDbContext.OutInfoHistory.AsNoTracking().AsEnumerable<BaseOut>();
            //outInfos = outInfos.Concat(history);

            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                outInfos = outInfos.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            if (!string.IsNullOrEmpty(TxtAddress.Text))
            {
                outInfos = outInfos.Where(x => x.Address.Contains(TxtAddress.Text) ||
                                         x.OrgName.Contains(TxtAddress.Text));
            }

            Pager.LongPage.AllCount = outInfos.Count();
            Pager.LongPage.Search = TxtMailNO.Text + TxtAddress;
            Pager.InitButton();

            MailDataGrid.ItemsSource = outInfos
                .OrderByDescending(x => x.ID)
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void PositionBtn_Click(object sender, RoutedEventArgs e)
        {
            var pWin = new ProgressBarWin
            {
                CallBack = CountryFind
            };
            pWin.ShowDialog();
            System.Windows.MessageBox.Show("划分成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Search();
        }

        private void CountryFind()
        {
            var cityPosition = _container.Resolve<CityPosition>();
            var mails = LongDbContext.OutInfo.Where(x => string.IsNullOrEmpty(x.CountryPosition)).ToList();
            foreach (var mail in mails)
            {
                cityPosition.CountryPositionByCityCode(mail);
                LongDbContext.OutInfo.Update(mail);
            }
            LongDbContext.SaveChanges();

            mails = LongDbContext.OutInfo.Where(x => string.IsNullOrEmpty(x.CountryPosition)).ToList();
            foreach (var mail in mails)
            {
                cityPosition.CountryPositionByParentCityCode(mail);
                LongDbContext.OutInfo.Update(mail);
            }
            LongDbContext.SaveChanges();
        }

        private void OutExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel|*.xlsx",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                new OutExcelHandle(dialog.FileName).Save();
            }
            Search();
        }
    }
}
