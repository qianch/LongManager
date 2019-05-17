using Autofac;
using LongManagerClient.Core.ClientDataBase;
using LongManagerClient.Pages;
using LongManagerClient.Pages.BLS;
using LongManagerClient.Pages.Car;
using LongManagerClient.Pages.City;
using LongManagerClient.Pages.In;
using LongManagerClient.Pages.Index;
using LongManagerClient.Pages.JiangSuOut;
using LongManagerClient.Pages.Label;
using LongManagerClient.Pages.Out;
using LongManagerClient.Pages.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LongManagerClient
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            //注册BasePage
            builder.RegisterType<Index>().As<BasePage>().Named<BasePage>("Index").SingleInstance();
            builder.RegisterType<Welcome>().As<BasePage>().Named<BasePage>("Welcome").SingleInstance();
            builder.RegisterType<UserList>().As<BasePage>().Named<BasePage>("UserList").SingleInstance();
            builder.RegisterType<CarList>().As<BasePage>().Named<BasePage>("CarList").SingleInstance();
            builder.RegisterType<LabelList>().As<BasePage>().Named<BasePage>("LabelList").SingleInstance();
            builder.RegisterType<CityList>().As<BasePage>().Named<BasePage>("CityList").SingleInstance();
            builder.RegisterType<OutList>().As<BasePage>().Named<BasePage>("OutList").SingleInstance();
            builder.RegisterType<JiangSuOutList>().As<BasePage>().Named<BasePage>("JiangSuOutList").SingleInstance();
            builder.RegisterType<BLSList>().As<BasePage>().Named<BasePage>("BLSList").SingleInstance();
            builder.RegisterType<OutSearch>().As<BasePage>().Named<BasePage>("OutSearch").SingleInstance();
            builder.RegisterType<InList>().As<BasePage>().Named<BasePage>("InList").SingleInstance();
            builder.RegisterType<InSearch>().As<BasePage>().Named<BasePage>("InSearch").SingleInstance();

            //当前登录用户
            builder.RegisterType<FrameUser>().As<FrameUser>().Named<FrameUser>("CurrentUser").SingleInstance();
        }
    }
}
