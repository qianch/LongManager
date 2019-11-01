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

namespace LongManagerClient.Pages.Config
{
    /// <summary>
    /// ConfigEdit.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigEdit : BaseWindow
    {
        public ConfigEdit()
        {
            InitializeComponent();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var key = ExtraData as string;
            var frameConfig = _longDBContext.FrameConfig.Where(x => x.RowGuid == key).FirstOrDefault();
            DataContext = frameConfig;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            _longDBContext.FrameConfig.Update(DataContext as FrameConfig);
            _longDBContext.SaveChanges();
            CallBack?.Invoke();
            MessageBox.Show("保存成功", "提示", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
