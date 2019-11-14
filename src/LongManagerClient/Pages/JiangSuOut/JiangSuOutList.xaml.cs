using Autofac;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
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
using Microsoft.EntityFrameworkCore;

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
            var jiangsu = LongDbContext.OutInfo.AsNoTracking().Where(x => x.CountryPosition == "38").AsEnumerable<BaseOut>();
            //var history = LongDbContext.OutInfoHistory.AsNoTracking().Where(x => x.CountryPosition == "38").AsEnumerable<BaseOut>();
            //jiangsu = jiangsu.Concat(history);

            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                jiangsu = jiangsu.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            if (!string.IsNullOrEmpty(TxtAddress.Text))
            {
                jiangsu = jiangsu.Where(x => x.Address.Contains(TxtAddress.Text) ||
                                         x.OrgName.Contains(TxtAddress.Text));
            }

            Pager.LongPage.AllCount = jiangsu.Count();
            Pager.LongPage.Search = TxtMailNO.Text + TxtAddress;
            Pager.InitButton();

            MailDataGrid.ItemsSource = jiangsu
                .Where(x => x.CountryPosition == "38")
                .OrderByDescending(x => x.ID)
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void PositionBtn_Click(object sender, RoutedEventArgs e)
        {
            PositionBtn.IsEnabled = false;
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
            PositionBtn.IsEnabled = true;
        }

        private void SynBtn_Click(object sender, RoutedEventArgs e)
        {
            SynBtn.IsEnabled = false;
            try
            {
                AutoPickDbContext.Database.CanConnect();
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                MessageBox.Show("无法连接到分拣机数据库", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                SynBtn.IsEnabled = true;
                return;
            }

            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush != 1);
            var serverbillExports = AutoPickDbContext.BillExport.AsNoTracking().ToList();

            int pageSize = 1000;
            int pages = (outInfos.Count() / pageSize);

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<OutInfo> subOutInfos = outInfos
                    .Take(pageSize)
                    .GroupBy(x => x.MailNO)
                    .Select(x => x.FirstOrDefault())
                    .ToList();

                foreach (var outInfo in subOutInfos)
                {
                    var billExport = new BillExport
                    {
                        BarCode = outInfo.MailNO,
                        DestAddress = outInfo.Address,
                        CountryBinCode = "10" + outInfo.CountryPosition.PadLeft(2, '0'),
                        BinCode = "10" + outInfo.JiangSuPosition.PadLeft(2, '0'),
                        CityName = outInfo.OrgName,
                        CreateDateTime = Convert.ToDateTime(outInfo.PostDate)
                    };

                    int count = serverbillExports.Where(x => x.BarCode == billExport.BarCode).Count();
                    if (count == 0)
                    {
                        AutoPickDbContext.BillExport.Add(billExport);
                    }
                    else
                    {
                        AutoPickDbContext.BillExport.Update(billExport);
                    }

                    outInfo.IsPush = 1;
                    LongDbContext.OutInfo.Update(outInfo);
                }

                AutoPickDbContext.SaveChanges();
                LongDbContext.SaveChanges();
            }

            MessageBox.Show("同步成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            SynBtn.IsEnabled = true;
        }

        private void NoSyncBtn_Click(object sender, RoutedEventArgs e)
        {
            MailDataGrid.ItemsSource = LongDbContext.OutInfo.AsNoTracking().Where(x => x.CountryPosition == "38" && x.IsPush != 1).Take(50).ToList();
        }

        private void MoveToHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            MoveToHistory();
        }

        private void MoveToHistory()
        {
            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush == 1 || x.CountryPosition != "38");
            var historys = LongDbContext.OutInfo.ToList();
            int pageSize = 1000;
            int pages = (outInfos.Count() / pageSize);

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<OutInfo> subInInfos = outInfos.Take(pageSize).ToList();
                foreach (var info in subInInfos)
                {
                    var history = new OutInfoHistory
                    {
                        RowGuid = info.RowGuid,
                        AddDate = info.AddDate,
                        PostDate = info.PostDate,
                        MailNO = info.MailNO,
                        OrgName = info.OrgName,
                        Consignee = info.Consignee,
                        Phone = info.Phone,
                        IsPush = info.IsPush,
                        Address = info.Address,
                        BelongOfficeName = info.BelongOfficeName,
                        CountryPosition = info.CountryPosition,
                        JiangSuPosition = info.JiangSuPosition
                    };
                    LongDbContext.OutInfoHistory.Add(history);
                }
            }
            LongDbContext.OutInfo.RemoveRange(outInfos);
            LongDbContext.SaveChanges();
            MessageBox.Show("已转移到历史库", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
