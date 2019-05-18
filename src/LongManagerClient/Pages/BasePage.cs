using Autofac;
using log4net;
using LongManagerClient.Core.ClientDataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LongManagerClient.Pages
{
    public class BasePage : Page
    {
        public BasePage()
        {

        }
        public ILog _log { get; set; }
        public LongClientDbContext _longDBContext { get; set; }
        protected readonly static IContainer _container = App.Container;
        public object ExtraData { get; set; }
    }
}
