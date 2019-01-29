using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace LongManager.Core
{
    public class GlobalCache
    {
        public static GlobalCache Instance { get; } = new GlobalCache();
        public Dictionary<string, Page> AllPages { get; } = new Dictionary<string, Page>();
        public Frame Frame { get; set; }
    }
}
