using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LongManagerClient.Command
{
    public static class LongManagerClientCommands
    {
        public static RoutedUICommand Exit = new RoutedUICommand("Exit", "Exit", typeof(LongManagerClientCommands));
        public static RoutedUICommand OpenTabCommand = new RoutedUICommand("OpenTabCommand", "OpenTabCommand", typeof(LongManagerClientCommands));
        public static RoutedUICommand PrintTabToPdfCommand = new RoutedUICommand("PrintTabToPdfCommand", "PrintTabToPdfCommand", typeof(LongManagerClientCommands));
        public static RoutedUICommand CustomCommand = new RoutedUICommand("CustomCommand", "CustomCommand", typeof(LongManagerClientCommands));
        public static RoutedUICommand MenuCommand = new RoutedUICommand("MenuCommand", "MenuCommand", typeof(LongManagerClientCommands));
        public static RoutedUICommand OpenPageCommand = new RoutedUICommand("OpenPageCommand", "OpenPageCommand", typeof(LongManagerClientCommands));
    }
}
