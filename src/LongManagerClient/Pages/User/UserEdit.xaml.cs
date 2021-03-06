﻿using Autofac;
using LongManagerClient.Core;
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

namespace LongManagerClient.Pages.User
{
    /// <summary>
    /// UserEdit1.xaml 的交互逻辑
    /// </summary>
    public partial class UserEdit : BaseWindow
    {
        public UserEdit()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var key = ExtraData as string;
            var frameUser = _longDBContext.FrameUser.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = frameUser;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _longDBContext.FrameUser.Update(DataContext as FrameUser);
            _longDBContext.SaveChanges();
            CallBack?.Invoke();
            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
