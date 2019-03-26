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
        protected readonly static ILog _log = LogManager.GetLogger(typeof(BasePage));
        protected LongClientDbContext _longDBContext = new LongClientDbContext();
        public object ExtraData { get; set; }
    }
}
