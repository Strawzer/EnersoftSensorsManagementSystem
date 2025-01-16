using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Repositories
{
    /// <summary>
    /// Repository for managing Sensor data operations.
    /// </summary>
    public class SensorRepository : ISensorRepository
    {
        private readonly PostgresSensorDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SensorRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public SensorRepository(PostgresSensorDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all sensors from the database.
        /// </summary>
        /// <returns>A collection of sensors.</returns>
        public async Task<IEnumerable<Sensor>> GetAllAsync()
        {
            return await _context.Sensors
                                 .Include(s => s.Type) // Include related SensorType
                                 .ToListAsync();
        }

        /// <summary>
        /// Retrieves a sensor by its identifier.
        /// </summary>
        /// <param name="id">The sensor identifier.</param>
        /// <returns>The sensor with the specified identifier, or null if not found.</returns>
        public async Task<Sensor> GetByIdAsync(int id)
        {
            return await _context.Sensors
                                 .Include(s => s.Type) // Include related SensorType
                                 .FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// Adds a new sensor to the database.
        /// </summary>
        /// <param name="sensor">The sensor to add.</param>
        public async Task AddAsync(Sensor sensor)
        {
            await _context.Sensors.AddAsync(sensor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing sensor in the database.
        /// </summary>
        /// <param name="sensor">The sensor to update.</param>
        public async Task UpdateAsync(Sensor sensor)
        {
            _context.Sensors.Update(sensor);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a sensor from the database by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the sensor to delete.</param>
        public async Task DeleteAsync(int id)
        {
            var sensor = await _context.Sensors.FindAsync(id);
            if (sensor != null)
            {
                _context.Sensors.Remove(sensor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
