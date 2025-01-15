using EnersoftSensorsManagementSystem.Core.Entities;

namespace EnersoftSensorsManagementSystem.Core.Interfaces;

public interface ISensorTypeRepository
{
    Task<IEnumerable<SensorType>> GetAllAsync();
    Task<SensorType> GetByIdAsync(int id);
    Task AddAsync(SensorType sensorType);
    Task UpdateAsync(SensorType sensorType);
    Task DeleteAsync(int id);
}