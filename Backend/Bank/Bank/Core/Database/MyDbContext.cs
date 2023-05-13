using Microsoft.EntityFrameworkCore;
using System;


namespace Bank.Core.Database
{
    public class MyDbContext : DbContext
    {

        public DbSet<User>? Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        }

    }
}
