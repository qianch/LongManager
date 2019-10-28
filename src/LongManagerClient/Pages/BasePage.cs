using Autofac;
using log4net;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Core.ServerDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LongManagerClient.Pages
{
    public abstract class BasePage : Page
    {
        public BasePage(){}
        public ILog _log { get; set; }
        public LongClientDbContext LongDbContext { get; set; }
        public AutoPickDbContext AutoPickDbContext { get; set; }
        protected readonly static IContainer _container = App.Container;
        public object ExtraData { get; set; }
    }
}
