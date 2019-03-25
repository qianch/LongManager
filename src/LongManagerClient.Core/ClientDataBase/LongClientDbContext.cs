using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LongManagerClient.Core.ClientDataBase
{
    public class LongClientDbContext : DbContext
    {
        private static readonly string frameConnection = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DB/LongClient.db")};";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(frameConnection);
        }

        public DbSet<FrameConfig> FrameConfig { get; set; }
        public DbSet<FrameUser> FrameUser { get; set; }
        public DbSet<CarBasicInfo> Car { get; set; }
        public DbSet<LabelBasicInfo> Label { get; set; }
        public DbSet<MailBasicInfo> Mail { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
    }
}
