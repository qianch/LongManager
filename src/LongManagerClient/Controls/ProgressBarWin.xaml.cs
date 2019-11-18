using LongManagerClient.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LongManagerClient.Controls
{
    /// <summary>
    /// ProgressBarWin.xaml 的交互逻辑
    /// </summary>
    public partial class ProgressBarWin : BaseWindow
    {
        public ProgressBarWin()
        {
            InitializeComponent();
        }


        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() => { Process(); });
            Task.Run(() =>
            {
                CallBack?.Invoke();
                Dispatcher.Invoke(Close);
            });
        }

        public void Process()
        {
            int max = 100;
            for (int i = 0; i <= max; i++)
            {
                progressBar.Dispatcher.Invoke(new Action<DependencyProperty, object>(progressBar.SetValue), System.Windows.Threading.DispatcherPriority.Background, ProgressBar.ValueProperty, Convert.ToDouble(i));
                if (i == 100)
                {
                    i = 1;
                }
                Thread.Sleep(1000);
            }
        }
    }
}
