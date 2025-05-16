using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyApiProject.Models;


namespace MyApiProject.Data
{
    public class AppDataBase : DbContext
    {
        public AppDataBase(DbContextOptions<AppDataBase> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<Department>()
                .HasIndex(e => e.Name)
                .IsUnique();

        }

    }
}