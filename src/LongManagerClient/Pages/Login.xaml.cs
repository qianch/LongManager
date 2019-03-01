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

namespace LongManagerClient.Pages
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : BaseWindow
    {
        private FrameUser _frameUser = new FrameUser();
        public Login()
        {
            InitializeComponent();

            DataContext = _frameUser;
        }

        private void InBtn_Click(object sender, RoutedEventArgs e)
        {
            _frameUser.Password = TxtPassword.Password;
            if (string.IsNullOrEmpty(_frameUser.UserName))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(_frameUser.Password))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int count = _longDBContext.FrameUsers.Where(x => x.UserName == _frameUser.UserName && x.Password == _frameUser.Password).ToList().Count();

            if (count == 1)
            {
                DialogResult = true;
                GlobalCache.Instance.FrameUser = _longDBContext.FrameUsers.Where(x => x.UserName == _frameUser.UserName && x.Password == _frameUser.Password).FirstOrDefault();
                MessageBox.Show("登录成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("用户名与密码不匹配，请重新输入", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

        }

        private void OutBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
