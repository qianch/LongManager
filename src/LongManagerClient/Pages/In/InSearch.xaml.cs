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
        private int _lastPage = 300;
        private int _currentPage = 0;

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
                if (e.Url.ToString() == _inMail + "cx" && _currentPage <= _lastPage)
                {
                    _currentPage++;
                }

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
                   //县市
                   var result = {{id:'张家港市',text:'张家港市',code:'320582'}}
                   $('#receiverCountyCode').val('320582');
                   $('#receiverCountyName').select2('data',result);

                   //信息来源
                   $('dataSource').prop('selectedIndex', 0);

                   //查询时间
                   //$('#postStartTime').val('2019-04-19');         

                   seachSubmit();
                }}
                
                if('{e.Url}' == '{_inMail}'+'cx'){{         
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
                               jsObject.saveInAddress(mailNO,address,orgName,consignee);
                           }}
                       }}
                   }}
                }}";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
