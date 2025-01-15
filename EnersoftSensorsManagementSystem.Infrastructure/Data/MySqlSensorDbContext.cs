using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class MySqlSensorDbContext : SensorDbContext
{
    public MySqlSensorDbContext(DbContextOptions<MySqlSensorDbContext> options) : base(options) { }
}
