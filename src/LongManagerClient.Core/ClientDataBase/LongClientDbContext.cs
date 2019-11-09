using LongManagerClient.Core.EFLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class LongClientDbContext : DbContext
    {
        private static readonly string _connection = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB/LongClient.db")};";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_connection);
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new EFLoggerProvider());
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }

        public DbSet<FrameConfig> FrameConfig { get; set; }
        public DbSet<FrameUser> FrameUser { get; set; }
        public DbSet<CarBasicInfo> Car { get; set; }
        public DbSet<LabelBasicInfo> Label { get; set; }
        public DbSet<InInfo> InInfo { get; set; }
        public DbSet<InInfoHistory> InInfoHistory { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
        public DbSet<CityInfo> CityInfo { get; set; }
        public DbSet<OutInfo> OutInfo { get; set; }
        public DbSet<OutInfoHistory> OutInfoHistory { get; set; }
        public DbSet<BLSOutInfo> BLSInfo { get; set; }
    }
}
