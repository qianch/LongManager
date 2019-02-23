using CefSharp;
using LongManager.Core.JSObject;
using LongManager.Core.ModelBinding;
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

namespace LongManager.Pages.EMS
{
    /// <summary>
    /// EMSSearch.xaml 的交互逻辑
    /// </summary>
    public partial class EMSSearch : BasePage
    {
        public EMSSearch()
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
        }

        private void BasePage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Browser.Load("https://10.3.131.164/cas/login");
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                var script = @"
                var userName = document.getElementById('username');
                if(userName != null){
                   userName.value='21566200admin';
                   var password = document.getElementById('password');
                   password.value='zjg123456';
                   document.getElementById('login').click();
                }else{
                   window.location.href='https://10.3.131.164/pcs-tc-web/a/mailQuery/toMail';
                };

                var wayBillNo = document.getElementById('wayBillNo');
                if(wayBillNo != null){
                   wayBillNo.value ='XE84425824531';
                   document.getElementById('btnSubmit').click();
                };

                var tables = document.getElementsByTagName('table');
                if(tables != null && tables.length >0 ){
                    var addressTable = tables[1];
                    var rows = addressTable.rows;
                    if(rows.length > 0 && rows[1].cells.length > 0){
                        var address = rows[1].cells[1].innerHTML;
                        jsObject.saveAddress('XE84425824531',address);
                    }
                }";
                Browser.ExecuteScriptAsync(script);
            }
        }
    }
}
