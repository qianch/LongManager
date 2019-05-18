using Autofac;
using LongManagerClient.Core;
using LongManagerClient.Core.BSL;
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

namespace LongManagerClient.Pages.BLS
{
    /// <summary>
    /// BLSList.xaml 的交互逻辑
    /// </summary>
    public partial class BLSList : BasePage
    {
        public BLSList()
        {
            InitializeComponent();

            Pager.PageIndexChange += Pager_PageIndexChange;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = _longDBContext.BLSInfo.Count();
            Pager.InitButton();
            MailDataGrid.ItemsSource = _longDBContext.BLSInfo
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            MailDataGrid.ItemsSource = _longDBContext.BLSInfo
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var mails = _longDBContext.BLSInfo.AsEnumerable();
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
            var cityPosition = _container.Resolve<CityPosition>();
            var mails = _longDBContext.BLSInfo.Where(x => x.BelongOfficeName == null).ToList();
            foreach (var mail in mails)
            {
                cityPosition.CountryPositionByCityCode(mail);
                _longDBContext.BLSInfo.Update(mail);
            }

            mails = _longDBContext.BLSInfo.Where(x => x.BelongOfficeName == null).ToList();
            foreach (var mail in mails)
            {
                cityPosition.CountryPositionByParentCityCode(mail);
                _longDBContext.BLSInfo.Update(mail);
            }

            _longDBContext.SaveChanges();
            System.Windows.MessageBox.Show("全国格口划分完成", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void BLSExcel_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "Excel|*.xlsx",
                CheckFileExists = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                new BLSExcelHandle(dialog.FileName).Save();
            }
        }
    }
}
