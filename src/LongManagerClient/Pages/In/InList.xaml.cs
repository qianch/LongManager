﻿using LongManagerClient.Core.ServerDataBase;
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
            Pager.LongPage.AllCount = LongDbContext.InInfo.Count();
            Pager.InitButton();
            MailDataGrid.ItemsSource = LongDbContext.InInfo
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            MailDataGrid.ItemsSource = LongDbContext.InInfo
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var mails = LongDbContext.InInfo.AsEnumerable();
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

        private void SynBtn_Click(object sender, RoutedEventArgs e)
        {
            var InInfos = LongDbContext.InInfo.Where(x => x.IsPush != 1).ToList();

            foreach (var info in InInfos)
            {
                var entryBill = new EntryBill
                {
                    BarCode = info.MailNO,
                    DestAddress = info.Address
                };
                AutoPickDbContext.EntryBill.Add(entryBill);

                info.IsPush = 1;
                LongDbContext.InInfo.Update(info);
            }

            AutoPickDbContext.SaveChanges();
            LongDbContext.SaveChanges();
           
            MessageBox.Show("同步成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
