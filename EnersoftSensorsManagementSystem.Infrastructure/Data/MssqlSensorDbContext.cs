using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class MssqlSensorDbContext : SensorDbContext
{
    public MssqlSensorDbContext(DbContextOptions<MssqlSensorDbContext> options) : base(options) { }
}