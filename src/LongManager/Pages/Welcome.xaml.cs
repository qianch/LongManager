using CefSharp;
using CefSharp.Wpf;
using LongManager.Core.ModelBinding;
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

namespace LongManager.Pages
{
    /// <summary>
    /// Welcom.xaml 的交互逻辑
    /// </summary>
    public partial class Welcome : BasePage
    {
        public Welcome()
        {
            InitializeComponent();

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            var bindingOptions = new BindingOptions()
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                // MethodInterceptor = new MethodInterceptorLogger()
            };
            Browser.RegisterAsyncJsObject("jsObject", new CallbackObjectForJs(), bindingOptions);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Browser.Address = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Htmls/pages/map.html");
        }

        private void CallBrowser_Click(object sender, RoutedEventArgs e)
        {
            Browser.ExecuteScriptAsync("browserAlert", "browserAlert");
        }

        private void CallBrowserGet_Click(object sender, RoutedEventArgs e)
        {
            Task<CefSharp.JavascriptResponse> javascriptResponse = Browser.EvaluateScriptAsync("browserAlertReturn", "browserAlertReturn");
            if (javascriptResponse.Result.Success)
            {
                MessageBox.Show(javascriptResponse.Result.Result.ToString());
            }
        }
    }

    public class CallbackObjectForJs
    {
        public string name = "";
        public void showMsg(string msg)
        {
            MessageBox.Show(msg);
        }
    }
}
