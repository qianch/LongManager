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
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            Pager.LongPage.AllCount = _longDBContext.CityInfo.Count();
            Pager.InitButton();
            CityDataGrid.ItemsSource = _longDBContext.CityInfo
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void Pager_PageIndexChange(object sender, EventArgs e)
        {
            CityDataGrid.ItemsSource = _longDBContext.CityInfo
                .Skip(Pager.LongPage.PageSize * (Pager.LongPage.PageIndex - 1))
                .Take(Pager.LongPage.PageSize)
                .ToList();
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {
            var citys = _longDBContext.CityInfo.AsEnumerable();
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
    }
}
