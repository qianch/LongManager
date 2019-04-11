using CefSharp;
using LongManagerClient.CEF;
using LongManagerClient.Core.CEF.JSObject;
using LongManagerClient.ModelBinding;
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

namespace LongManagerClient.Pages.In
{
    /// <summary>
    /// InSearch.xaml 的交互逻辑
    /// </summary>
    public partial class InSearch : BasePage
    {
        private string _login = "https://10.3.131.164/cas/login";
        private string _inMail = "https://10.3.131.164/pcsnct-web/a/pcs/mailpretreatment/list";

        public InSearch()
        {
            InitializeComponent();

            var bindingOptions = new BindingOptions
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                MethodInterceptor = new MethodInterceptorLogger()
            };
            Browser.RegisterJsObject("jsObject", new CallbackObjectForJs(), bindingOptions);
            Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            Browser.FrameLoadEnd += Browser_FrameLoadEnd;
            Browser.MenuHandler = new LongCEFMenuHandler();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Browser.Load(_login);
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var script = $@"
                //登录
                if( '{e.Url}' == '{_login}'){{
                   var userName = document.getElementById('username');
                   if(userName != null){{
                      userName.value='21560019admin';
                      var password = document.getElementById('password');
                      password.value='zjg123456';
                      document.getElementById('login').click();
                   }}else{{
                      window.location.href='{_inMail}';
                   }};
                }}
                
                //设置寄达县
                if('{e.Url}' == '{_inMail}'){{
                   var receiverCountyName = $('#s2id_receiverCountyName');
                   var chosen = receiverCountyName.find('.select2-chosen');
                   if(chosen.text() == '请选择寄达县市'){{
                      receiverCountyName.click();
                   }}
                   $('#btnSubmit').click();
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
