using EnersoftSensorsManagementSystem.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

/// <summary>
/// Database context for managing sensors and sensor types.
/// </summary>
public class SensorDbContext : DbContext
{
    /// <summary>
    /// Users table
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// Sensors table.
    /// </summary>
    public DbSet<Sensor> Sensors { get; set; }

    /// <summary>
    /// Sensor types table.
    /// </summary>
    public DbSet<SensorType> SensorTypes { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorDbContext"/> class.
    /// </summary>
    /// <param name="options">Database context options.</param>
    public SensorDbContext(DbContextOptions options) : base(options) { }

    /// <summary>
    /// Configures the entity relationships and seeds initial data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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

        // Configure the Sensor-SensorType relationship
        modelBuilder.Entity<Sensor>()
            .HasOne(s => s.Type)
            .WithMany(t => t.Sensors)
            .HasForeignKey(s => s.TypeId)
            .OnDelete(DeleteBehavior.Cascade);

        // Seed SensorType data
        modelBuilder.Entity<SensorType>().HasData(
            new SensorType { Id = 1, Name = "ESP Module" },
            new SensorType { Id = 2, Name = "Soil Moisture Sensor" },
            new SensorType { Id = 3, Name = "Raspberry Camera" },
            new SensorType { Id = 4, Name = "DHT11 Temperature and Humidity Sensor" }
        );

        // Seed Sensor data
        modelBuilder.Entity<Sensor>().HasData(
            new Sensor { Id = 1, Name = "ESP32 Dev Board", Location = "Greenhouse 1", InstallationDate = DateTime.Now.AddMonths(-6), IsActive = true, TypeId = 1 },
            new Sensor { Id = 2, Name = "Soil Moisture Probe", Location = "Field A", InstallationDate = DateTime.Now.AddMonths(-3), IsActive = true, TypeId = 2 },
            new Sensor { Id = 3, Name = "Raspberry Pi Camera", Location = "Surveillance Room", InstallationDate = DateTime.Now.AddYears(-1), IsActive = false, TypeId = 3 },
            new Sensor { Id = 4, Name = "DHT11 Sensor", Location = "Living Room", InstallationDate = DateTime.Now.AddMonths(-2), IsActive = true, TypeId = 4 }
        );
    }
}
