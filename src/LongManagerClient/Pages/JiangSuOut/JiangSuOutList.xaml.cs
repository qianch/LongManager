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
using LongManagerClient.Controls;

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
            var pWin = new ProgressBarWin
            {
                CallBack = JiangSuFind
            };
            pWin.ShowDialog();
            MessageBox.Show("划分成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Search();
        }

        private void JiangSuFind()
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
        }

        private void SynBtn_Click(object sender, RoutedEventArgs e)
        {
            var pWin = new ProgressBarWin
            {
                CallBack = Sync
            };
            pWin.ShowDialog();
            MessageBox.Show("同步结束", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Search();
        }

        private void Sync()
        {
            try
            {
                AutoPickDbContext.Database.CanConnect();
            }
            catch (Exception ex)
            {
                _log.Error("连接出错", ex);
                MessageBox.Show("无法连接到分拣机数据库", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush != 1 && x.CountryPosition == "38" && !string.IsNullOrEmpty(x.JiangSuPosition));
            var barCodes = AutoPickDbContext.BillExport.AsNoTracking().Select(x => x.BarCode).ToList();

            int pageSize = 1000;
            int pages = (outInfos.Count() / pageSize);

            try
            {
                List<string> mailNOs = new List<string>();

                for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
                {
                    List<OutInfo> subOutInfos = outInfos
                        .Take(pageSize)
                        .ToList();

                    foreach (var outInfo in subOutInfos)
                    {
                        var countryBinCode = outInfo.CountryPosition == null ? "" : "10" + outInfo.CountryPosition.PadLeft(2, '0');
                        var binCode = outInfo.JiangSuPosition == null ? "" : "10" + outInfo.JiangSuPosition.PadLeft(2, '0');
                        var billExport = new BillExport
                        {
                            BarCode = outInfo.MailNO,
                            DestAddress = outInfo.Address,
                            //CountryBinCode = countryBinCode,
                            BinCode = binCode,
                            CityName = outInfo.OrgName,
                            CreateDateTime = Convert.ToDateTime(outInfo.PostDate)
                        };

                        int serverCount = barCodes.Where(x => x == billExport.BarCode).Count();
                        int localCount = mailNOs.Where(x => x == billExport.BarCode).Count();
                        if (serverCount == 0 && localCount == 0)
                        {
                            mailNOs.Add(billExport.BarCode);
                            AutoPickDbContext.BillExport.Add(billExport);
                        }

                        outInfo.IsPush = 1;
                        LongDbContext.OutInfo.Update(outInfo);
                    }

                    AutoPickDbContext.SaveChanges();
                    LongDbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _log.Error("同步出错", ex);
                return;
            }
        }

        private void NoSyncBtn_Click(object sender, RoutedEventArgs e)
        {
            MailDataGrid.ItemsSource = LongDbContext.OutInfo.AsNoTracking().Where(x => x.CountryPosition == "38" && x.IsPush != 1).Take(50).ToList();
        }

        private void MoveToHistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            var pWin = new ProgressBarWin
            {
                CallBack = MoveToHistory
            };
            pWin.ShowDialog();
            MessageBox.Show("转移成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            Search();
        }

        private void MoveToHistory()
        {
            LongDbContext.OutInfoHistory.RemoveRange(LongDbContext.OutInfoHistory.ToList());
            LongDbContext.SaveChanges();

            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush == 1 || x.CountryPosition != "38");
            int pageSize = 10000;
            int pages = (outInfos.Count() / pageSize);

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<OutInfo> subOutInfos = outInfos.Take(pageSize).ToList();
                foreach (var info in subOutInfos)
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
                LongDbContext.OutInfo.RemoveRange(subOutInfos);
                LongDbContext.SaveChanges();
            }
        }
    }
}
