using HogwartsScheduleAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HogwartsScheduleAPI.Data
{
    public class HogwartsDbContext : DbContext
    {
        public HogwartsDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<House> Houses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(c => c.Students)
                .WithMany(s => s.Courses)
                .UsingEntity("CoursesStudents");
        }
    }
}
