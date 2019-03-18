using CefSharp;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.CEF;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
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
using System.Windows.Shapes;
using Zen.Barcode;

namespace LongManagerClient.Pages.Car
{
    /// <summary>
    /// CarNOPrint.xaml 的交互逻辑
    /// </summary>
    public partial class CarNOPrint : BaseWindow
    {
        public CarBasicInfo _carBasicInfo = new CarBasicInfo();

        public CarNOPrint()
        {
            InitializeComponent();
            Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
            Browser.MenuHandler = new LongCEFMenuHandler();
        }

        private void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Browser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var key = ExtraData as string;
            _carBasicInfo = _longDBContext.Cars.Where(x => x.RowGuid == key).FirstOrDefault();

            var barStream = new MemoryStream();
            var barcode128 = BarcodeDrawFactory.Code128WithChecksum;
            var barImg = barcode128.Draw(_carBasicInfo.CarNO, 40);
            barImg.Save(barStream, ImageFormat.Jpeg);
            var barBase64 = Convert.ToBase64String(barStream.ToArray());

            var qrStream = new MemoryStream();
            var qrcode = BarcodeDrawFactory.CodeQr;
            var qrImg = qrcode.Draw(_carBasicInfo.CarNO, qrcode.GetDefaultMetrics(40));
            qrImg.Save(qrStream, ImageFormat.Jpeg);
            var qrBase64 = Convert.ToBase64String(qrStream.ToArray());

            Browser.LoadHtml($@"<html>
                                  <body style='width:700px;'>
                                    <div style='margin-left:200px;'>
                                       <div style='margin:10px;'>
                                          <img src='data:image/jpeg;base64,{barBase64}'></img>
                                       </div>
                                       <div style='margin:10px;'>
                                          <img src='data:image/jpeg;base64,{qrBase64}'></img>
                                       </div>
                                       <div style='margin:10px;'>{_carBasicInfo.CarNO}</div>
                                    </div>
                                  </body>
                                </html>");
        }

        private void PrintCarNO_Click(object sender, RoutedEventArgs e)
        {
            Browser.ExecuteScriptAsync("window.print", "");
        }
    }
}
