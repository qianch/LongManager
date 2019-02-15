using log4net;
using LongManager.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LongManager.Pages
{
    public class BaseWindow : Window
    {
        protected ILog _log = LogManager.GetLogger("Window");
        protected LongDbContext _longDBContext = new LongDbContext();
        public object ExtraData { get; set; }

        public BaseWindow()
        {
            Width = 800;
            Height = 450;
            ShowInTaskbar = false;
            ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Icon = new BitmapImage(new Uri("Images/favicon.ico", UriKind.Relative));
        }
    }
}
