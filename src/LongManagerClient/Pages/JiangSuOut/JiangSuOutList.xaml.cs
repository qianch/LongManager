using Autofac;
using LongManagerClient.Core;
using LongManagerClient.Core.ServerDataBase;
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
            Pager.LongPage.AllCount = LongDbContext.OutInfo.Where(x => x.CountryPosition == "38").Count();
            Pager.InitButton();
            MailDataGrid.ItemsSource = LongDbContext.OutInfo
                .Where(x => x.CountryPosition == "38")
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            ListChange();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            ListChange();
        }

        protected override void Search() 
        {
            ListChange();
        }

        private void ListChange()
        {
            var mails = LongDbContext.OutInfo.Where(x => x.CountryPosition == "38").AsEnumerable();
            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                mails = mails.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            if (!string.IsNullOrEmpty(TxtAddress.Text))
            {
                mails = mails.Where(x => x.Address.Contains(TxtAddress.Text) ||
                                         x.OrgName.Contains(TxtAddress.Text));
            }

            Pager.LongPage.AllCount = mails.Count();
            Pager.LongPage.Search = TxtMailNO.Text + TxtAddress;
            Pager.InitButton();

            MailDataGrid.ItemsSource = mails
                .Where(x => x.CountryPosition == "38")
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void PositionBtn_Click(object sender, RoutedEventArgs e)
        {
            var cityPosition = _container.Resolve<CityPosition>();
            //长三角地区邮件
            var mails = LongDbContext.OutInfo.Where(x => x.CountryPosition == "38" && string.IsNullOrEmpty(x.JiangSuPosition)).ToList();
            foreach (var mail in mails)
            {
                cityPosition.JiangSuPositionByCityCode(mail);
                LongDbContext.OutInfo.Update(mail);
            }
            LongDbContext.SaveChanges();
            MessageBox.Show("长三角格口划分完成", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void SynBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AutoPickDbContext.Database.CanConnect();
            }
            catch (Exception)
            {
                MessageBox.Show("无法连接到分拣机数据库", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush != 1 && x.JiangSuPosition != null).ToList();

            foreach (var outInfo in outInfos)
            {
                var billExport = new BillExport
                {
                    BarCode = outInfo.MailNO,
                    DestAddress = outInfo.Address,
                    BinCode = "10" + outInfo.JiangSuPosition.PadLeft(2, '0'),
                    CityName = outInfo.OrgName
                };

                outInfo.IsPush = 1;
                AutoPickDbContext.BillExport.Add(billExport);
                LongDbContext.OutInfo.Update(outInfo);
            }

            AutoPickDbContext.SaveChanges();
            LongDbContext.SaveChanges();

            MessageBox.Show("同步成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
