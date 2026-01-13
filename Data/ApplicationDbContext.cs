using Microsoft.EntityFrameworkCore;
using Mini_Task_Manager_Application.Models;

namespace Mini_Task_Manager_Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskItem> Tasks { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TaskItem>().Property(t => t.IsCompleted).HasDefaultValue(false);
        }
    }
}
