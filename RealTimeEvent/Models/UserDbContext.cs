using Microsoft.EntityFrameworkCore;

namespace RealTimeEvent.Models;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
    {
         Database.EnsureCreated();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();


            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsRequired();

            entity.HasMany(u => u.Messages)
                .WithOne(m => m.User)
                .HasForeignKey(m => m.UserId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(e => e.Text)
                .HasMaxLength(1000)
                .IsRequired();
        });
    }
}