using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;

namespace TaskManager.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectTask> Tasks { get; set; }
        public DbSet<TaskHistory> TaskHistories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        // NÃO inclua BaseEvent aqui - não é uma entidade

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Role).IsRequired().HasConversion<string>();

                entity.HasMany(u => u.Projects)
                      .WithOne()
                      .HasForeignKey(p => p.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ignore DomainEvents collection
                entity.Ignore(nameof(User.DomainEvents));
            });

            // Project configuration
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);

                entity.HasMany(p => p.Tasks)
                      .WithOne()
                      .HasForeignKey(t => t.ProjectId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ignore DomainEvents collection
                entity.Ignore(nameof(Project.DomainEvents));
            });

            // Task configuration
            modelBuilder.Entity<ProjectTask>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Title).IsRequired().HasMaxLength(100);
                entity.Property(t => t.Description).HasMaxLength(1000);
                entity.Property(t => t.Status).IsRequired().HasConversion<string>();
                entity.Property(t => t.Priority).IsRequired().HasConversion<string>();

                entity.HasMany(t => t.History)
                      .WithOne()
                      .HasForeignKey(th => th.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(t => t.Comments)
                      .WithOne()
                      .HasForeignKey(c => c.TaskId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Ignore DomainEvents collection
                entity.Ignore(nameof(ProjectTask.DomainEvents));
            });

            // TaskHistory configuration
            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(th => th.Id);
                entity.Property(th => th.FieldChanged).IsRequired().HasMaxLength(50);
                entity.Property(th => th.OldValue).HasMaxLength(500);
                entity.Property(th => th.NewValue).HasMaxLength(500);
            });

            // Comment configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired().HasMaxLength(1000);
            });
        }
    }
}