using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class PostgresSensorDbContext : DbContext
{
    /// <summary>
    /// Sensors table.
    /// </summary>
    public DbSet<Sensor> Sensors { get; set; }

    /// <summary>
    /// Sensor types table.
    /// </summary>
    public DbSet<SensorType> SensorTypes { get; set; }

    public PostgresSensorDbContext(DbContextOptions<PostgresSensorDbContext> options) : base(options) { }

    /// <summary>
    /// Configures the entity relationships and seeds initial data.
    /// </summary>
    /// <param name="modelBuilder">The model builder.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply configurations
        modelBuilder.ApplyConfiguration(new SensorConfiguration());
        modelBuilder.ApplyConfiguration(new SensorTypeConfiguration());


        // Seed data
        modelBuilder.Entity<SensorType>().HasData(
            new SensorType { Id = 1, Name = "ESP Module" },
            new SensorType { Id = 2, Name = "Soil Moisture Sensor" },
            new SensorType { Id = 3, Name = "Raspberry Camera" },
            new SensorType { Id = 4, Name = "DHT11 Temperature and Humidity Sensor" }
        );

        modelBuilder.Entity<Sensor>().HasData(
            new Sensor
            {
                Id = 1,
                Name = "ESP32 Dev Board",
                Location = "Greenhouse 1",
                IsActive = true,
                TypeId = 1
            },
            new Sensor
            {
                Id = 2,
                Name = "Soil Moisture Probe",
                Location = "Field A",
                IsActive = true,
                TypeId = 2
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