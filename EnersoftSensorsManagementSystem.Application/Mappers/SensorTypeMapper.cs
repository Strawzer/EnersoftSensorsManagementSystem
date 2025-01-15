using EnersoftSensorsManagementSystem.Application.DTOs;
using EnersoftSensorsManagementSystem.Core.Entities;

namespace EnersoftSensorsManagementSystem.Application.Mappers;

public static class SensorTypeMapper
{
    public static SensorTypeDto ToDto(SensorType sensorType)
    {
        return new SensorTypeDto
        {
            Id = sensorType.Id,
            Name = sensorType.Name
        };
    }

    public static SensorType ToEntity(SensorTypeCreateUpdateDto dto)
    {
        return new SensorType
        {
            Name = dto.Name
        };
    }
}
