using Microsoft.EntityFrameworkCore;
using System;


namespace Bank.Core.Database
{
    public class MyDbContext : DbContext
    {

        public DbSet<User>? Users { get; set; }
        public DbSet<Platba>? Platba { get; set; }
        public DbSet<Ucet>? Ucty { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=mydatabase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Platba>().ToTable("Platba");
            modelBuilder.Entity<Ucet>().ToTable("Ucet");
        }

    }
}
