﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LongManagerWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:5001")
                .ConfigureLogging((context, logger) =>
                {
                    logger.AddFilter("System", LogLevel.Warning);
                    logger.AddFilter("Microsoft", LogLevel.Warning);
                    logger.AddLog4Net();
                })
                .UseStartup<Startup>();
    }
}
