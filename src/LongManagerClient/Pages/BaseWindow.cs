﻿using log4net;
using LongManagerClient.Core.ClientDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LongManagerClient.Pages
{
    public class BaseWindow : Window
    {
        protected ILog _log = LogManager.GetLogger("Window");
        protected LongClientDbContext _longDBContext = new LongClientDbContext();
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
