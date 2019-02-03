using LongManager.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LongManager.Core
{
    public class GlobalCache
    {
        public static GlobalCache Instance { get; } = new GlobalCache();
        public Dictionary<string, Page> AllPages { get; } = new Dictionary<string, Page>();
        public Frame Frame { get; set; } = new Frame();
        public FrameUser FrameUser { get; set; } = new FrameUser();
    }
}
