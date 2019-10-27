using Autofac;
using LongManagerClient.Core.ClientDataBase;
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
using System.Windows.Shapes;

namespace LongManagerClient.Pages.City
{
    /// <summary>
    /// CityEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CityEdit : BaseWindow
    {
        public CityEdit()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var key = ExtraData as string;
            var cityInfo = _longDBContext.CityInfo.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = cityInfo;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _longDBContext.CityInfo.Update(DataContext as CityInfo);
            _longDBContext.SaveChanges();
            _container.ResolveNamed<BasePage>("CityList").refresh();
            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
