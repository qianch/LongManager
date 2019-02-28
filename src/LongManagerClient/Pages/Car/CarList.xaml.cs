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

namespace LongManagerClient.Pages.Car
{
    /// <summary>
    /// CarList.xaml 的交互逻辑
    /// </summary>
    public partial class CarList : BasePage
    {
        public CarList()
        {
            InitializeComponent();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            CarDataGrid.ItemsSource = _longDBContext.Cars.ToList();
        }

        private void ViewBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var window = new CarEdit
            {
                ExtraData = editButton.Tag,
            };
            window.ShowDialog();
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            var editButton = sender as Button;
            var window = new CarNOPrint
            {
                ExtraData = editButton.Tag,
            };
            window.ShowDialog();
        }
    }
}
