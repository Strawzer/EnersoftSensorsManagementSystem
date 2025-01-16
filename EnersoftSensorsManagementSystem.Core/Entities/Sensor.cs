namespace EnersoftSensorsManagementSystem.Core.Entities;

/// <summary>
/// Sensor entity
/// </summary>
public class Sensor
{
    /// <summary>
    /// Sensor identifier
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Sensor Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Sensor installation location
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Is active sensor
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Sensor type identifier foreign key
    /// </summary>
    public int TypeId { get; set; }

    /// <summary>
    /// Sensor type navigation property
    /// </summary>
    public SensorType Type { get; set; }
}