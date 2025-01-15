using EnersoftSensorsManagementSystem.Core.Entities;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using EnersoftSensorsManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EnersoftSensorsManagementSystem.Infrastructure.Repositories;

/// <summary>
/// Repository for managing SensorType data operations.
/// </summary>
public class SensorTypeRepository : ISensorTypeRepository
{
    private readonly SensorDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="SensorTypeRepository"/> class.
    /// </summary>
    /// <param name="context">The database context.</param>
    public SensorTypeRepository(SensorDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all sensor types from the database.
    /// </summary>
    /// <returns>A collection of sensor types.</returns>
    public async Task<IEnumerable<SensorType>> GetAllAsync()
    {
        return await _context.SensorTypes.ToListAsync();
    }

    /// <summary>
    /// Retrieves a sensor type by its identifier.
    /// </summary>
    /// <param name="id">The sensor type identifier.</param>
    /// <returns>The sensor type with the specified identifier, or null if not found.</returns>
    public async Task<SensorType> GetByIdAsync(int id)
    {
        return await _context.SensorTypes.FindAsync(id);
    }

    /// <summary>
    /// Adds a new sensor type to the database.
    /// </summary>
    /// <param name="sensorType">The sensor type to add.</param>
    public async Task AddAsync(SensorType sensorType)
    {
        await _context.SensorTypes.AddAsync(sensorType);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing sensor type in the database.
    /// </summary>
    /// <param name="sensorType">The sensor type to update.</param>
    public async Task UpdateAsync(SensorType sensorType)
    {
        _context.SensorTypes.Update(sensorType);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a sensor type from the database by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the sensor type to delete.</param>
    public async Task DeleteAsync(int id)
    {
        var sensorType = await _context.SensorTypes.FindAsync(id);
        if (sensorType != null)
        {
            _context.SensorTypes.Remove(sensorType);
            await _context.SaveChangesAsync();
        }
    }
}
