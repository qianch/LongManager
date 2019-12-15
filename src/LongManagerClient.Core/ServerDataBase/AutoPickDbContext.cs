﻿using LongManagerClient.Core.EFLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerClient.Core.ServerDataBase
{
    public class AutoPickDbContext : DbContext
    {
        private static readonly string _connection = "Server=106.12.85.7;Database=AutoPick;Uid=root;Pwd=123@epoint;Character Set=utf8;";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connection);
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public DbSet<EntryBill> EntryBill { get; set; }
        public DbSet<BillExport> BillExport { get; set; }
    }
}
