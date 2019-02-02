using log4net;
using LongManager.Core.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LongManager.Pages
{
    public class BaseWindow : Window
    {
        protected ILog _log = LogManager.GetLogger("Window");
        protected LongDbContext _longDBContext = new LongDbContext();
    }
}
