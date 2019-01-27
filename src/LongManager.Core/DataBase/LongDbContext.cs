using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LongManager.Core.DataBase
{
    public class LongDbContext : DbContext
    {
        private static readonly string frameConnection = $"Data Source={Path.Combine(new[] { AppDomain.CurrentDomain.BaseDirectory, "DB", "Long.db" })};";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(frameConnection);
        }

        public DbSet<FrameConfig> FrameConfigs { get; set; }
    }
}
