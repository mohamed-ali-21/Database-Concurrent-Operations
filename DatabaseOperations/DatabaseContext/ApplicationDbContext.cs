using DatabaseOperations.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace DatabaseOperations.DatabaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Student> Students{ get; set; }
        public DbSet<Doctor> Doctors{ get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasKey(s => s.Id);
            modelBuilder.Entity<Student>().Property(s => s.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Doctor>().HasKey(s => s.Id);
            modelBuilder.Entity<Doctor>().Property(s => s.Id).ValueGeneratedOnAdd();
        }
    }
}
