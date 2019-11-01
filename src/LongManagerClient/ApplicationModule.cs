using Autofac;
using log4net;
using LongManagerClient.Core;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Core.ServerDataBase;
using LongManagerClient.Pages;
using LongManagerClient.Pages.BLS;
using LongManagerClient.Pages.Car;
using LongManagerClient.Pages.City;
using LongManagerClient.Pages.Config;
using LongManagerClient.Pages.In;
using LongManagerClient.Pages.Index;
using LongManagerClient.Pages.JiangSuOut;
using LongManagerClient.Pages.Label;
using LongManagerClient.Pages.Out;
using LongManagerClient.Pages.User;
using LongManagerClient.Port;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace LongManagerClient
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LongClientDbContext>().As<LongClientDbContext>().InstancePerDependency();
            builder.RegisterType<AutoPickDbContext>().As<AutoPickDbContext>().InstancePerLifetimeScope();
            //注册BasePage
            builder.RegisterType<Index>().As<BasePage>().Named<BasePage>("Index").SingleInstance().PropertiesAutowired();
            builder.RegisterType<Welcome>().As<BasePage>().Named<BasePage>("Welcome").SingleInstance().PropertiesAutowired();
            builder.RegisterType<UserList>().As<BasePage>().Named<BasePage>("UserList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<CarList>().As<BasePage>().Named<BasePage>("CarList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<LabelList>().As<BasePage>().Named<BasePage>("LabelList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<CityList>().As<BasePage>().Named<BasePage>("CityList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<OutList>().As<BasePage>().Named<BasePage>("OutList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<JiangSuOutList>().As<BasePage>().Named<BasePage>("JiangSuOutList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<BLSList>().As<BasePage>().Named<BasePage>("BLSList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<OutSearch>().As<BasePage>().Named<BasePage>("OutSearch").SingleInstance().PropertiesAutowired();
            builder.RegisterType<InList>().As<BasePage>().Named<BasePage>("InList").SingleInstance().PropertiesAutowired();
            builder.RegisterType<InSearch>().As<BasePage>().Named<BasePage>("InSearch").SingleInstance().PropertiesAutowired();
            builder.RegisterType<ConfigList>().As<BasePage>().Named<BasePage>("ConfigList").SingleInstance().PropertiesAutowired();

            //当前登录用户
            builder.RegisterInstance(new FrameUser()).As<FrameUser>().Named<FrameUser>("CurrentUser").SingleInstance();

            string[] portNames = SerialPort.GetPortNames();
            if (portNames.Count() > 0)
            {
                builder.RegisterInstance(new LongSerialPort(portNames[0])).As<LongSerialPort>().SingleInstance();
            }

            //格口划分方法
            builder.RegisterType<CityPosition>().As<CityPosition>().PropertiesAutowired();
            builder.RegisterInstance(LogManager.GetLogger(typeof(App))).As<ILog>();
        }
    }
}
