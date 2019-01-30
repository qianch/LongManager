using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace LongManager.Core
{
    public class GlobalCache
    {
        public static GlobalCache Instance { get; } = new GlobalCache();
        public Dictionary<string, Page> AllPages { get; } = new Dictionary<string, Page>();
        public Frame Frame { get; set; } = new Frame();
        public NavigationWindow NavigationWindow { get; set; } = new NavigationWindow
        {
            ShowsNavigationUI = false,
            Width = 800,
            Height = 450,
            ShowInTaskbar = false,
            ResizeMode = ResizeMode.NoResize
        };
    }
}
