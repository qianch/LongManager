using CefSharp;
using LongManagerClient.CEF;
using LongManagerClient.Core.CEF.JSObject;
using LongManagerClient.Core.ClientDataBase;
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
        private string _login = "";
        private string _logout = "";
        private string _outMail = "";
        private string _ajax = "";
        private const int _lastPage = 500;
        private const int _pageSize = 50;
        private readonly string _today = DateTime.Now.ToString("yyyy-MM-dd");
        private bool _flag = false;

        public OutSearch()
        {
            InitializeComponent();

            var bindingOptions = new BindingOptions
            {
                Binder = BindingOptions.DefaultBinder.Binder,
                MethodInterceptor = new MethodInterceptorLogger()
            };
            Browser.RegisterJsObject("jsObject", new CallbackObjectForJs(), bindingOptions);

            Browser.FrameLoadEnd += Browser_FrameLoadEnd;
            Browser.MenuHandler = new LongCEFMenuHandler();
            //Browser.IsEnabled = false;
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {
            string newUrl = LongDbContext.FrameConfig.Where(x => x.ConfigName == "NewUrl").First().ConfigValue;

            var showDevToolsConfig = LongDbContext.FrameConfig.Where(x => x.ConfigName == "ShowDevTools").FirstOrDefault() ?? new FrameConfig();
            var enable = "1";
            if (showDevToolsConfig.ConfigValue == enable)
            {
                _flag = true;
            }

            _login = $"https://{newUrl}/cas/login";
            _logout = $"https://{newUrl}/cas/logout";
            _outMail = $"https://{newUrl}/pickup-web/a/pickup/waybillquery/main";
            _ajax = $"https://{newUrl}/pickup-web/a/pickup/waybillquery/querybase";
            if (!Browser.IsBrowserInitialized)
            {
                Browser.Address = _login;
            }
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (_flag)
            {
                Browser.ShowDevTools();
                _flag = false;
            }

            if (e.Frame.IsMain)
            {
                if (e.Url.ToString() == "chrome-error://chromewebdata/")
                {
                    Browser.ExecuteScriptAsync("alert('新一代无法登陆，请检查网络是否正常。')");
                    return;
                }

                var script = $@"
                //登录
                if( '{e.Url}'.indexOf('{_login}') > -1 ){{
                   var userName = document.getElementById('username');
                   if(userName != null){{
                      //userName.value='21566400admin';
                      //var password = document.getElementById('password');
                      //password.value='xyd123456';
                      //document.getElementById('login').click();
                   }}else{{
                      window.location.href='{_outMail}';
                   }};
                }}

                if( '{e.Url}' == '{_outMail}'){{
                   for(var i=0;i<={_lastPage};i++){{
                      getOutInfo(i,{_pageSize});
                      console.log('出口抓取，当前页数：'+ i);
                   }}
                }}
                
                function getOutInfo(pageNo,pageSize){{
                  $.ajax({{
                     type:'POST',
                     url:'{_ajax}',
                     data:{{
                       'postOrgNo':'',
                       'orgDrdsCode':'',
                       'wayBillNo':'',
                       'postState':'',
                       'bizOccurDateStart':'{_today} 00:00:00',
                       'bizOccurDateEnd':'{_today} 23:59:59',
                       'senderNo':'',
                       'sender':'',
                       'senderWarehouseId':'',
                       'senderWarehouseName':'',
                       'postPersonNo':'',
                       'settlementMode':'',
                       'ioType':'',
                       'bizProductNo':'',
                       'allowSealingFlag':'',
                       'isFeedFlag':'',
                       'codFlag':'',
                       'feeDateStart':'',
                       'feeDateEnd':'',
                       'oneBillFlag':'',
                       'insuranceFlag':'',
                       'packaging':'',
                       'pickupPersonNo':'',
                       'receiverProvinceNo':'',
                       'receiverProvinceName':'',
                       'senderLinker':'',
                       'senderMobile':'',
                       'receiverLinker':'',
                       'receiverMobile':'',
                       'transferType':'',
                       'pageNo':pageNo,
                       'pageSize':pageSize
                     }},
                     dataType:'JSON',
                     success:function(result){{
                        if(result.resCode == '0001'){{
                           return;
                        }}
                        
                        if(result.detail.length > 0){{
                           jsObject.saveOutInfo(JSON.stringify(result.detail));
                        }}
                     }}
                   }});
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }

        private void GoLogin_Click(object sender, RoutedEventArgs e)
        {
            GoLoginPage();
        }

        private void GoLoginPage()
        {
            Browser.ExecuteScriptAsync($"alert('返回登陆页面');window.location.href='{_logout}?serviceurl={_login}';");
        }
    }
}
