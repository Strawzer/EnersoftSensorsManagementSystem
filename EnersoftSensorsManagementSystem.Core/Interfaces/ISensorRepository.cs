using EnersoftSensorsManagementSystem.Core.Entities;

namespace EnersoftSensorsManagementSystem.Core.Interfaces;

public interface ISensorRepository
{
    Task<IEnumerable<Sensor>> GetAllAsync();
    Task<Sensor> GetByIdAsync(int id);
    Task AddAsync(Sensor sensor);
    Task UpdateAsync(Sensor sensor);
    Task DeleteAsync(int id);
}