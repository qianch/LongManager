using LongManager.Core.DataBase;
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

namespace LongManager.Pages.User
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
            var frameUser = _longDBContext.FrameUsers.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = frameUser;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _longDBContext.FrameUsers.Update(DataContext as FrameUser);
            _longDBContext.SaveChanges();
        }
    }
}
