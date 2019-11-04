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
        private string _login = "";
        private string _logout = "";
        private string _inMail = "";
        private const int _lastPage = 500;
        private const int _pageSize = 50;
        private string _date = DateTime.Now.ToString("yyyy-MM-dd");
        private int _currentPage = 0;
        private bool _flag = true;

        public InSearch()
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
            _login = $"https://{newUrl}/cas/login";
            _inMail = $"https://{newUrl}/pcsnct-web/a/pcs/mailpretreatment/list";
            _logout = $"https://{newUrl}/cas/logout";

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
                if (e.Url.ToString() == _inMail + "cx" && _currentPage <= _lastPage)
                {
                    _currentPage++;
                }

                var script = $@"
                //登录
                if( '{e.Url}'.indexOf('{_login}') > -1){{
                   var userName = document.getElementById('username');
                   if(userName != null){{
                      //userName.value='21560019admin';
                      //var password = document.getElementById('password');
                      //password.value='zjg123456';
                      //document.getElementById('login').click();
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
                   var dataSource1 = {{id:'0',text:'邮政'}}
                   var dataSource2 = {{id:'1',text:'速递'}}
                   $('#dataSource').select2('data',dataSource2);
                   $('#handleFlag').select2('data',dataSource2);

                   //查询时间
                   //$('#postStartTime').val('{_date}');         

                   seachSubmit();
                }}
                
                if('{e.Url}' == '{_inMail}'+'cx'){{         
                   page({_currentPage},{_pageSize},'');
                   var tables = document.getElementsByTagName('table');
                   if(tables != null && tables.length > 0 ){{
                       var addressTable = tables[2];
                       //console.log(addressTable.innerHTML);

                       var rows = addressTable.rows;

                       if(rows.length > 0){{
                           var rlength = rows.length;
                           var clength = rows[1].cells.length;

                           /* for(var i = 0 ; i < rlength; i++){{
                               for(var j=0; j < clength; j++){{
                                  console.log('位置信息___i:'+ i + 'j:' + j +'内容:'+rows[i].cells[j].innerHTML);
                               }}
                           }} */
                           
                           for(var i = 0 ; i < rlength; i++){{                                
                               var mailNO = rows[i].cells[2].innerHTML;
                               mailNO = mailNO.length > 12 ? mailNO.substring(0,13) : mailNO;
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

        private void GoLogin_Click(object sender, RoutedEventArgs e)
        {
            Browser.ExecuteScriptAsync($"alert('返回登陆页面');window.location.href='{_logout}?serviceurl={_login}';");
        }

        private void SelectDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _date = SelectDate.SelectedDate.Value.ToString("yyyy-MM-dd");
        }
    }
}
