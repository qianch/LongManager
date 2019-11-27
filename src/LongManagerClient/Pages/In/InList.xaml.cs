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

namespace LongManagerClient.Pages.In
{
    /// <summary>
    /// InList.xaml 的交互逻辑
    /// </summary>
    public partial class InList : BasePage
    {
        public InList()
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

        protected void Search()
        {
            var infos = LongDbContext.InInfo.AsNoTracking().AsEnumerable<BaseIn>();
            //var history = LongDbContext.InInfoHistory.AsNoTracking().AsEnumerable<BaseIn>();
            //infos = infos.Concat(history);

            if (!string.IsNullOrEmpty(TxtMailNO.Text))
            {
                infos = infos.Where(x => x.MailNO.Contains(TxtMailNO.Text));
            }

            if (!string.IsNullOrEmpty(TxtAddress.Text))
            {
                infos = infos.Where(x => x.Address.Contains(TxtAddress.Text) ||
                                         x.OrgName.Contains(TxtAddress.Text));
            }

            Pager.LongPage.AllCount = infos.Count();
            Pager.LongPage.Search = TxtMailNO.Text;
            Pager.InitButton();

            MailDataGrid.ItemsSource = infos
                .OrderByDescending(x => x.ID)
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
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
                _log.Error(ex.ToString());
                MessageBox.Show("无法连接到分拣机数据库", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var inInfos = LongDbContext.InInfo.Where(x => x.IsPush != 1);
            var serverEntryBills = AutoPickDbContext.EntryBill.AsNoTracking().ToList();

            int pageSize = 1000;
            int pages = (inInfos.Count() / pageSize);

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<InInfo> subInInfos = inInfos
                    .Take(pageSize)
                    .GroupBy(x => x.MailNO)
                    .Select(x => x.FirstOrDefault())
                    .ToList();

                foreach (var info in subInInfos)
                {
                    var entryBill = new EntryBill
                    {
                        BarCode = info.MailNO,
                        DestAddress = info.Address,
                        PresortPost = info.OrgName,
                        CreateDateTime = Convert.ToDateTime(info.PostDate)
                    };

                    int count = serverEntryBills.Where(x => x.BarCode == entryBill.BarCode).Count();
                    if (count == 0)
                    {
                        AutoPickDbContext.EntryBill.Add(entryBill);
                    }

                    info.IsPush = 1;
                    LongDbContext.InInfo.Update(info);
                }

                AutoPickDbContext.SaveChanges();
                LongDbContext.SaveChanges();
            }
        }

        private void NoSyncBtn_Click(object sender, RoutedEventArgs e)
        {
            MailDataGrid.ItemsSource = LongDbContext.InInfo.AsNoTracking().Where(x => x.IsPush != 1).Take(50).ToList();
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
            var inInfos = LongDbContext.InInfo.Where(x => x.IsPush == 1);
            int pageSize = 10000;
            int pages = (inInfos.Count() / pageSize);

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<InInfo> subInInfos = inInfos.Take(pageSize).ToList();
                foreach (var info in subInInfos)
                {
                    var history = new InInfoHistory
                    {
                        RowGuid = info.RowGuid,
                        AddDate = info.AddDate,
                        PostDate = info.PostDate,
                        MailNO = info.MailNO,
                        OrgName = info.OrgName,
                        Consignee = info.Consignee,
                        Phone = info.Phone,
                        IsPush = info.IsPush,
                        Address = info.Address
                    };
                    LongDbContext.InInfoHistory.Add(history);
                }
                LongDbContext.InInfo.RemoveRange(subInInfos);
                LongDbContext.SaveChanges();
            }
        }
    }
}
