using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LongManagerWeb.Core.WebDataBase
{
    public class LongWebDbContext : DbContext
    {
        public LongWebDbContext(DbContextOptions<LongWebDbContext> options)
            : base(options)
        {
        }

        public DbSet<FrameUser> FrameUser { get; set; }
        public DbSet<LoginHistory> LoginHistory { get; set; }
    }
}
