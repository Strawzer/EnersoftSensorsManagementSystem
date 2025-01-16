using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class MssqlSensorDbContext : DbContext
{
    /// <summary>
    /// Users table.
    /// </summary>
    public DbSet<User> Users { get; set; }

    public MssqlSensorDbContext(DbContextOptions<MssqlSensorDbContext> options) : base(options) { }

    /// <summary>
    /// Configures the entity relationships and seeds initial data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new UserConfiguration());

        // Seed User data
        var passwordHasher = new PasswordHasher<User>();
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                PasswordHash = passwordHasher.HashPassword(null, "password"),
                Role = "Admin"
            },
            new User
            {
                Id = 2,
                Username = "user",
                PasswordHash = passwordHasher.HashPassword(null, "password"),
                Role = "User"
            }
        );

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging(); // Shows the actual query and parameters in logs
        optionsBuilder.LogTo(Console.WriteLine); // Logs SQL queries to console
    }
}