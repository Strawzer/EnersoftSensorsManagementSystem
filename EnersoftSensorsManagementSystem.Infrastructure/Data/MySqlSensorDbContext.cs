using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Data;

public class MySqlSensorDbContext : DbContext
{
    public MySqlSensorDbContext(DbContextOptions<MySqlSensorDbContext> options) : base(options) { }
}
