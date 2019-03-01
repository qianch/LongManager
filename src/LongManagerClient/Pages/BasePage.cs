using log4net;
using LongManagerClient.Core.DataBase;
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
        protected static readonly ILog _log = LogManager.GetLogger("Page");
        protected LongDbContext _longDBContext = new LongDbContext();
        public object ExtraData { get; set; }
    }
}
