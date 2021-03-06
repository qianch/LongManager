﻿using Autofac;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Port;
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
            _carBasicInfo = _longDBContext.Car.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = _carBasicInfo;

            LabelCBox.ItemsSource = _longDBContext.Label.ToList();
            LabelCBox.DisplayMemberPath = "LabelNO";
            LabelCBox.SelectedValuePath = "LabelNO";
        }

        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            var isRegistered = App.Container.IsRegistered<LongSerialPort>();
            if (!isRegistered)
            {
                MessageBox.Show("没有找到对应的发射单元", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var longSerialPort = App.Container.Resolve<LongSerialPort>();
            if (DelayTime.Text.ToArray().Any(x => Char.IsNumber(x)) &&
                ActionTime.Text.ToArray().Any(x => Char.IsNumber(x)))
            {
                longSerialPort.SendOrderData(_carBasicInfo.CarNO, LabelCBox.SelectedValue.ToString(), DelayTime.Text, ActionTime.Text, orientation.Text == "正转" ? "0" : "1");
                MessageBox.Show("数据已经发送", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("延迟时间格式不正确", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
