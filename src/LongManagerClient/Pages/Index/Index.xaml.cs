using LongManagerClient.CEF;
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

namespace LongManagerClient.Pages.Index
{
    /// <summary>
    /// Index.xaml 的交互逻辑
    /// </summary>
    public partial class Index : BasePage
    {
        public Index()
        {
            InitializeComponent();

            Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            Browser.MenuHandler = new LongCEFMenuHandler();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Browser.Load(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Htmls/pages/index.html"));
        }
    }
}
