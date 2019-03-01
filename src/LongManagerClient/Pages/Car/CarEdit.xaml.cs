using LongManagerClient.Core;
using LongManagerClient.Core.DataBase;
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

namespace LongManagerClient.Pages.Car
{
    /// <summary>
    /// CarEdit.xaml 的交互逻辑
    /// </summary>
    public partial class CarEdit : BaseWindow
    {
        public CarBasicInfo _carBasicInfo = new CarBasicInfo();
        public CarEdit()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var key = ExtraData as string;
            _carBasicInfo = _longDBContext.Cars.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = _carBasicInfo;

            LabelCBox.ItemsSource = _longDBContext.Labels.ToList();
            LabelCBox.DisplayMemberPath = "LabelNO";
            LabelCBox.SelectedValuePath = "LabelNO";
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            var longSerialPort = GlobalCache.Instance.LongSerialPort;
            if (longSerialPort == null)
            {
                MessageBox.Show("没有找到对应的发射单元", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                if (DelayTime.Text.ToArray().Any(x => Char.IsNumber(x)) &&
                    ActionTime.Text.ToArray().Any(x => Char.IsNumber(x)))
                {
                    longSerialPort.SendOrderData(_carBasicInfo.CarNO, LabelCBox.SelectedValue.ToString(), DelayTime.Text, ActionTime.Text);
                    MessageBox.Show("数据已经发送", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MessageBox.Show("延迟时间格式不正确", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
