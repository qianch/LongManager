using log4net;
using LongManager.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LongManager.Pages
{
    public class BasePage : Page
    {
        protected static readonly ILog _log = LogManager.GetLogger("log");
        protected LongDbContext _longDBContext = new LongDbContext();

        public BasePage()
        {

        }

        public object ExtraData { get; set; }
    }
}
