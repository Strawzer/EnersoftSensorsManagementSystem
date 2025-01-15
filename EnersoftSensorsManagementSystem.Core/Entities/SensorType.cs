namespace EnersoftSensorsManagementSystem.Core.Entities;

/// <summary>
/// Sensor type entity
/// </summary>
public class SensorType
{
    /// <summary>
    /// Sensor type identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sensor type name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Type related sensors navigation property
    /// </summary>
    public ICollection<Sensor> Sensors { get; set; } = new List<Sensor>();
}
