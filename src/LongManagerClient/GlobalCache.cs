using Autofac;
using Autofac.Core;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Port;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace LongManagerClient
{
    public class GlobalCache
    {
        public Frame Frame { get; set; } = new Frame();
        public LongSerialPort LongSerialPort { get; set; }

        public static IContainer Container()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).PropertiesAutowired();
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterInstance(new GlobalCache()).As<GlobalCache>().SingleInstance();
            return builder.Build();
        }
    }
}
