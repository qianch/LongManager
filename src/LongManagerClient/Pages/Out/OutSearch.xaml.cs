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
        private string _login = "https://10.3.131.164/cas/login";
        private string _outMail = "https://10.3.131.164/pickup-web/a/pickup/waybillquery/main";
        private int _lastPage = 300;
        private int _currentPage = 0;

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
            Browser.Load("_login");
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                if (e.Url.ToString() == _outMail && _currentPage <= _lastPage)
                {
                    _currentPage++;
                }

                var script = $@"
                //登录
                if( '{e.Url}' == '{_login}'){{
                   var userName = document.getElementById('username');
                   if(userName != null){{
                      userName.value='21566400admin';
                      var password = document.getElementById('password');
                      password.value='xyd123456';
                      document.getElementById('login').click();
                   }}else{{
                      window.location.href='{_outMail}';
                   }};
                }}
                
                if('{e.Url}' == '{_outMail}'){{         
                   page({_currentPage},10,'');
                   var tables = document.getElementsByTagName('table');
                   if(tables != null && tables.length > 0 ){{
                       var addressTable = tables[2];
                       console.log(addressTable.innerHTML);

                       var rows = addressTable.rows;
                       if(rows.length > 0){{
                           var rlength = rows.length;
                           var clength = rows[1].cells.length;
                           for(var i = 0 ; i < rlength; i++){{
                               for(var j=0; j < clength; j++){{
                                  console.log('位置信息___i:'+ i + 'j:' + j +'内容:'+rows[i].cells[j].innerHTML);
                               }}
                           }}
                           
                           for(var i = 0 ; i < rlength; i++){{                                
                               var mailNO = rows[i].cells[2].innerHTML;
                               var address = rows[i].cells[4].innerHTML;
                               var orgName = rows[i].cells[5].innerHTML;
                               var consignee = rows[i].cells[9].innerHTML;
                           }}
                       }}
                   }}
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
