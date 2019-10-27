﻿using System;
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

namespace LongManagerClient.Pages.City
{
    /// <summary>
    /// CityList.xaml 的交互逻辑
    /// </summary>
    public partial class CityList : BasePage
    {
        public CityList()
        {
            InitializeComponent();

            Pager.PageIndexChange += Pager_PageIndexChange;
            refresh = Search;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = LongDbContext.CityInfo.Count();
            Pager.InitButton();
            CityDataGrid.ItemsSource = LongDbContext.CityInfo
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            CityDataGrid.ItemsSource = LongDbContext.CityInfo
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        protected override void Search()
        {
            var citys = LongDbContext.CityInfo.AsNoTracking().AsEnumerable();
            if (!string.IsNullOrEmpty(TxtCityName.Text))
            {
                citys = citys.Where(x => x.CityName.Contains(TxtCityName.Text));
            }

            Pager.LongPage.AllCount = citys.Count();
            Pager.LongPage.Search = TxtCityName.Text;
            Pager.InitButton();

            CityDataGrid.ItemsSource = citys
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var window = new CityEdit
            {
                ExtraData = editButton.Tag,
            };
            window.ShowDialog();
        }
    }
}
