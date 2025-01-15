using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnersoftSensorsManagementSystem.Application.DTOs;

/// <summary>
/// Data Transfer Object for Sensor.
/// </summary>
public class SensorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public DateTime InstallationDate { get; set; }
    public bool IsActive { get; set; }
    public string TypeName { get; set; } = string.Empty;
}
