using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;


namespace Bank.Core.Database
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        { }

        public DbSet<User>? Users { get; set; }
        public DbSet<Platba>? Platba { get; set; }
        public DbSet<Ucet>? Ucty { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Platba>().ToTable("Platba");    
            modelBuilder.Entity<Ucet>().ToTable("Ucet");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder
            {
                DataSource = "mydatabase.db"
            };

            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        public void CreateDatabaseAndTables()
        {
            // Create the database if it doesn't exist
            Database.EnsureCreated();
        }


    }
}
