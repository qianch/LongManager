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
            _frameUser.UserPassword = TxtPassword.Password;
            if (string.IsNullOrEmpty(_frameUser.UserName))
            {
                MessageBox.Show("请输入用户名", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (string.IsNullOrEmpty(_frameUser.UserPassword))
            {
                MessageBox.Show("请输入密码", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            int count = _longDBContext.FrameUser.Where(x => x.UserName == _frameUser.UserName && x.UserPassword == _frameUser.UserPassword).ToList().Count();

            if (count == 1)
            {
                DialogResult = true;
                var frameUser = GlobalCache.Instance.FrameUser = _longDBContext.FrameUser.Where(x => x.UserName == _frameUser.UserName && x.UserPassword == _frameUser.UserPassword).FirstOrDefault();
                var loginHistory = new LoginHistory
                {
                    RowGuid = Guid.NewGuid().ToString(),
                    LoginUserName = frameUser.UserName,
                    LoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    IsPush = 0
                };
                _longDBContext.LoginHistory.Add(loginHistory);
                _longDBContext.SaveChanges();
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
