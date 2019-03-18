using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Port;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LongManagerClient
{
    public class GlobalCache
    {
        public static GlobalCache Instance { get; } = new GlobalCache();
        public Dictionary<string, Page> AllPages { get; } = new Dictionary<string, Page>();
        public Frame Frame { get; set; } = new Frame();
        public FrameUser FrameUser { get; set; } = new FrameUser();
        public LongSerialPort LongSerialPort { get; set; }
    }
}
