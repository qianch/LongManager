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
        private string _ajax = "https://10.3.131.164/pickup-web/a/pickup/waybillquery/querybase";
        private int _lastPage = 300;
        private string _today = DateTime.Now.ToString("yyyy-MM-dd");

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
                      userName.value='21566400admin';
                      var password = document.getElementById('password');
                      password.value='xyd123456';
                      document.getElementById('login').click();
                   }}else{{
                      window.location.href='{_outMail}';
                   }};
                }}

                if( '{e.Url}' == '{_outMail}'){{
                   for(var i=0;i<{_lastPage};i++){{
                      getOutInfo(i,10);
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
                        jsObject.saveOutInfo(JSON.stringify(result));
                     }}
                   }});
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
