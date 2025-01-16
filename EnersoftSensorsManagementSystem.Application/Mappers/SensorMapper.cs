using EnersoftSensorsManagementSystem.Application.DTOs;
using EnersoftSensorsManagementSystem.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnersoftSensorsManagementSystem.Application.Mappers;

public static class SensorMapper
{
    public static SensorDto ToDto(Sensor sensor)
    {
        return new SensorDto
        {
            Id = sensor.Id,
            Name = sensor.Name,
            Location = sensor.Location,
            IsActive = sensor.IsActive,
            TypeName = sensor.Type?.Name
        };
    }

    public static Sensor ToEntity(SensorCreateUpdateDto dto)
    {
        return new Sensor
        {
            Name = dto.Name,
            Location = dto.Location,
            IsActive = dto.IsActive,
            TypeId = dto.TypeId
        };
    }
}