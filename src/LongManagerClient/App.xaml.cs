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
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace LongManagerClient
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private ILog log = LogManager.GetLogger(typeof(App));
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            DispatcherUnhandledException += App_DispatcherUnhandledException;
            if (ExistRunningInstance())
            {
                Shutdown();
            }

            //监听com端口
            string[] portNames = SerialPort.GetPortNames();
            foreach (var portName in portNames)
            {
                GlobalCache.Instance.LongSerialPort = new LongSerialPort(portName);
            }

            //设置CefSharp
            var settings = new CefSettings
            {
                IgnoreCertificateErrors = true,
            };
            Cef.Initialize(settings);
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;

            //quartz
            new PushTask().Run().GetAwaiter().GetResult();
        }

        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            log.Error(e.Exception.ToString());
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
