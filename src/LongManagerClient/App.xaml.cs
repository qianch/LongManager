using Autofac;
using CefSharp;
using CefSharp.Wpf;
using log4net;
using LongManagerClient.Core;
using LongManagerClient.Core.QuartzJob;
using LongManagerClient.Port;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LongManagerClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private ILog _log = LogManager.GetLogger(typeof(App));
        public static IContainer Container { get; private set; }
        public static Frame Frame { get; set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            if (ExistRunningInstance())
            {
                Shutdown();
            }

            //设置CefSharp
            var settings = new CefSettings
            {
                IgnoreCertificateErrors = true,
            };
            Cef.Initialize(settings);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            //quartz
            //new PushTask().Run().GetAwaiter().GetResult();

            //autofac
            var builder = new ContainerBuilder();
            //builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule(new ApplicationModule());
            Container = builder.Build();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            _log.Error(e.Exception.ToString());
        }

        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private static bool ExistRunningInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] procList = Process.GetProcessesByName(currentProcess.ProcessName);

            foreach (Process proc in procList)
            {
                // Found a running instance
                if (proc.Id != currentProcess.Id)
                {
                    MessageBox.Show("程序已经运行!", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                    SetForegroundWindow(proc.MainWindowHandle);
                    return true;
                }
            }
            return false;
        }
    }
}
