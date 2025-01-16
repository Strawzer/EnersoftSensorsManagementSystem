using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using EnersoftSensorsManagementSystem.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace EnersoftSensorsManagementSystem.UnitTests;

public class SensorRepositoryTests
{
    private readonly PostgresSensorDbContext _context;
    private readonly SensorRepository _repository;

    public SensorRepositoryTests()
    {
        // Configure the in-memory database for PostgresSensorDbContext
        var options = new DbContextOptionsBuilder<PostgresSensorDbContext>()
            .UseInMemoryDatabase(databaseName: "SensorTestDb")
            .Options;

        // Initialize the DbContext and repository
        _context = new PostgresSensorDbContext(options);
        _repository = new SensorRepository(_context);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveSensor_WhenSensorExists()
    {
        // Arrange
        var sensor = new Sensor { Id = 1, Name = "Test Sensor", Location = "Lab" };
        _context.Sensors.Add(sensor);
        await _context.SaveChangesAsync();

        // Act
        await _repository.DeleteAsync(sensor.Id);

        // Assert
        var deletedSensor = await _context.Sensors.FindAsync(sensor.Id);
        deletedSensor.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldDoNothing_WhenSensorDoesNotExist()
    {
        // Arrange
        var nonExistentSensorId = 99;

        // Act
        await _repository.DeleteAsync(nonExistentSensorId);

        // Assert
        // No exception should be thrown, and the test should pass
        var sensorCount = await _context.Sensors.CountAsync();
        sensorCount.Should().Be(0);
    }
}
