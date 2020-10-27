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
        private int _lastPage = 800;
        private int _pageSize = 100;
        private string _date = "";
        private int _currentPage = 0;
        private bool _flag = false;

        public InSearch()
        {
            InitializeComponent();

            LastPage.Text = _lastPage.ToString();
            PageSize.Text = _pageSize.ToString();

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
            var newUrlCongfig = LongDbContext.FrameConfig.Where(x => x.ConfigName == "NewUrl").FirstOrDefault() ?? new FrameConfig();
            string newUrl = newUrlCongfig.ConfigValue;

            var showDevToolsConfig = LongDbContext.FrameConfig.Where(x => x.ConfigName == "ShowDevTools").FirstOrDefault() ?? new FrameConfig();
            var enable = "1";
            _currentPage = _lastPage;
            if (showDevToolsConfig.ConfigValue == enable)
            {
                _flag = true;
            }

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

            if (string.IsNullOrEmpty(_date))
            {
                _date = DateTime.Now.AddHours(-6).ToString("yyyy-MM-dd");
            }

            if (e.Frame.IsMain)
            {
                if (e.Url.ToString() == "chrome-error://chromewebdata/")
                {
                    Browser.ExecuteScriptAsync("alert('新一代无法登陆，请检查网络是否正常。')");
                    return;
                }

                if (_currentPage == 0)
                {
                    _currentPage = _lastPage;
                }

                if (e.Url.ToString() == _inMail + "cx")
                {
                    _currentPage--;
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
                   $('#postStartTime').val('{_date}');
                   console.log('日期：'+ '{_date}');

                   seachSubmit();
                }}
                
                if('{e.Url}' == '{_inMail}'+'cx'){{         
                   page({_currentPage},{_pageSize},'');
                   console.log('进口抓取，当前页数：'+ {_currentPage});
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
                           
                           var mails = [];
                           for(var i = 0 ; i < rlength; i++){{                                
                               var mailNO = rows[i].cells[2].innerHTML;
                               mailNO = mailNO.length > 12 ? mailNO.substring(0,13) : mailNO;

                               var address = rows[i].cells[4].innerHTML;
                               var orgName = rows[i].cells[5].innerHTML;
                               var consignee = rows[i].cells[9].innerHTML;

                               var mail = {{}};
                               mail.mailNO = mailNO;
                               mail.address = address;
                               mail.orgName = orgName;
                               mail.consignee = '';

                               mails.push(mail);
                           }}
                           jsObject.saveInAddress(JSON.stringify(mails),'{_date}');
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

        private void PageSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            _pageSize = Convert.ToInt32(PageSize.Text);
        }

        private void LastPage_TextChanged(object sender, TextChangedEventArgs e)
        {
            _lastPage = Convert.ToInt32(LastPage.Text);
        }
    }
}
