﻿using Autofac;
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
            var mails = LongDbContext.OutInfo.AsNoTracking().Where(x => x.CountryPosition == "38").AsEnumerable();
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
            catch (Exception)
            {
                MessageBox.Show("无法连接到分拣机数据库", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                SynBtn.IsEnabled = true;
                return;
            }

            var outInfos = LongDbContext.OutInfo.Where(x => x.IsPush != 1 && x.JiangSuPosition != null);
            var serverbillExports = AutoPickDbContext.BillExport.AsNoTracking().ToList();

            int pageSize = 1000;
            int pages = (outInfos.Count() / pageSize) + 1;

            for (int pageIndex = 0; pageIndex <= pages; pageIndex++)
            {
                List<OutInfo> subOutInfos = outInfos.Take(pageSize).ToList();

                foreach (var outInfo in subOutInfos)
                {
                    var billExport = new BillExport
                    {
                        BarCode = outInfo.MailNO,
                        DestAddress = outInfo.Address,
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
    }
}
