using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class PostgresSensorDbContext : SensorDbContext
{
    public PostgresSensorDbContext(DbContextOptions<PostgresSensorDbContext> options) : base(options) { }
}