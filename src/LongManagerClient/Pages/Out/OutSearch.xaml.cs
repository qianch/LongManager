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

namespace LongManagerClient.Pages.Out
{
    /// <summary>
    /// EMSSearch.xaml 的交互逻辑
    /// </summary>
    public partial class OutSearch : BasePage
    {
        private List<string> _mailNOList = new List<string>();
        private string _currentMialNO = "";
        public OutSearch()
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
            Browser.IsEnabled = false;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            _mailNOList = _longDBContext.OutInfo.Select(x => x.MailNO).ToList();
        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Browser.Load("https://10.3.131.164/cas/login");
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var outMail = "https://10.3.131.164/pcs-tc-web/a/mailQuery/toMail";
                var mailList = "https://10.3.131.164/pcs-tc-web/a/mailQuery/mailList";

                if (e.Url.ToString() == outMail && _mailNOList.Count() > 0)
                {
                    _currentMialNO = _mailNOList[0];
                    _mailNOList.Remove(_currentMialNO);
                }

                var script = $@"
                //登录
                if( '{e.Url}' == 'https://10.3.131.164/cas/login'){{
                   var userName = document.getElementById('username');
                   if(userName != null){{
                      userName.value='21566200admin';
                      var password = document.getElementById('password');
                      password.value='zjg123456';
                      document.getElementById('login').click();
                   }}else{{
                      window.location.href='{outMail}';
                   }};
                }}
                
                //邮件查询
                if('{e.Url}' == '{outMail}'){{
                   var wayBillNo = document.getElementById('wayBillNo');
                   if(wayBillNo != null){{
                      wayBillNo.value ='{_currentMialNO}';
                      document.getElementById('btnSubmit').click();
                   }};
                }}

                //查询结果
                if('{e.Url}' == '{mailList}'){{
                   var tables = document.getElementsByTagName('table');
                   if(tables != null && tables.length >0 ){{
                       var addressTable = tables[1];
                       var rows = addressTable.rows;
                       if(rows.length > 0 && rows[1].cells.length > 0){{
                           var address = rows[1].cells[1].innerHTML;
                           console.log('{_currentMialNO}',address);
                           jsObject.saveOutAddress('{_currentMialNO}',address);
                           if({_mailNOList.Count()} > 0){{
                              window.location.href='{outMail}';
                           }}
                       }}
                   }}
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
