using CefSharp;
using CefSharp.Wpf;
using LongManagerClient.CEF;
using LongManagerClient.Core.CEF.JSObject;
using LongManagerClient.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LongManagerClient.Pages
{
    /// <summary>
    /// Welcom.xaml 的交互逻辑
    /// </summary>
    public partial class Welcome : BasePage
    {
        public Welcome()
        {
            InitializeComponent();

            var bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                MethodInterceptor = new MethodInterceptorLogger()
            };
            Browser.RegisterAsyncJsObject("jsObject", new CallbackObjectForJs(), bindingOptions);
            Browser.Address = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Htmls/pages/welcome.html");
            Browser.MenuHandler = new LongCEFMenuHandler();
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void Search() 
        {

        }

        private void CallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Browser.ExecuteScriptAsync("browserAlert", "browserAlert");
        }

        private void CallBrowserGet_Click(object sender, RoutedEventArgs e)
        {
            Task<JavascriptResponse> javascriptResponse = Browser.EvaluateScriptAsync("browserAlertReturn", "browserAlertReturn");
            if (javascriptResponse.Result.Success)
            {
                MessageBox.Show(javascriptResponse.Result.Result.ToString(), "C#调用js的返回值", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
        }
    }
}
