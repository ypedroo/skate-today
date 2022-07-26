using Microsoft.EntityFrameworkCore;
using shouldISkateToday.Domain.Dtos;
using shouldISkateToday.Domain.Models;

namespace shouldISkateToday.Data.Contexts;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions options) : base(options)
    {

    }
    // Entities        
    public DbSet<User> Users { get; set; }
    
    // Entity Configurations
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.UserName).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.PasswordSalt).IsRequired();
            entity.Property(e => e.RefreshToken);
            entity.Property(e => e.RefreshTokenExpires);
            entity.Property(e => e.RefreshTokenCreated);


            entity.ToTable("Users");
        });
    }
}