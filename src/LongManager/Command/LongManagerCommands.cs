using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LongManager.Command
{
    public static class LongManagerCommands
    {
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(LongManagerCommands));
        public static RoutedUICommand OpenTabCommand = new RoutedUICommand("OpenTabCommand", "OpenTabCommand", typeof(LongManagerCommands));
        public static RoutedUICommand PrintTabToPdfCommand = new RoutedUICommand("PrintTabToPdfCommand", "PrintTabToPdfCommand", typeof(LongManagerCommands));
        public static RoutedUICommand CustomCommand = new RoutedUICommand("CustomCommand", "CustomCommand", typeof(LongManagerCommands));
    }
}
