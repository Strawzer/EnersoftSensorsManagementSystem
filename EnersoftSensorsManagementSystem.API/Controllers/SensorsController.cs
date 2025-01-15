using EnersoftSensorsManagementSystem.Application.DTOs;
using EnersoftSensorsManagementSystem.Application.Interfaces;
using EnersoftSensorsManagementSystem.Application.Mappers;
using EnersoftSensorsManagementSystem.Application.Validators;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.Results;
using Microsoft.Extensions.Caching.Memory;
using EnersoftSensorsManagementSystem.API.Exceptions;
using EnersoftSensorsManagementSystem.API.Models;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace EnersoftSensorsManagementSystem.API.Controllers
{
    /// <summary>
    /// API controller for managing sensors.
    /// </summary>
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiVersion("1.0")]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorRepository _repository;
        private readonly IMemoryCache _cache;
        private const string SensorsCacheKey = "GetAllSensors";

        public SensorsController(ISensorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorDto>>> GetAll()
        {
            // Try to get data from cache
            if (!_cache.TryGetValue(SensorsCacheKey, out IEnumerable<SensorDto> sensors))
            {
                // If not in cache, retrieve from database
                var sensorEntities = await _repository.GetAllAsync();
                sensors = sensorEntities.Select(SensorMapper.ToDto);

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                // Save data in cache
                _cache.Set(SensorsCacheKey, sensors, cacheOptions);
            }

            return Ok(sensors);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            // Remove cached data when a sensor is deleted
            _cache.Remove(SensorsCacheKey);

            return NoContent();
        }

        /// <summary>
        /// Retrieves a sensor by its identifier.
        /// </summary>
        /// <param name="id">The sensor identifier.</param>
        /// <returns>The sensor with the specified identifier, or a 404 status if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorDto>> GetById(int id)
        {
            var sensor = await _repository.GetByIdAsync(id);
            if (sensor == null)
                throw new NotFoundException($"Sensor with ID {id} was not found.");

            return Ok(SensorMapper.ToDto(sensor));
        }

        /// <summary>
        /// Adds a new sensor.
        /// </summary>
        /// <param name="sensorDto">The sensor data to add.</param>
        /// <returns>A 201 status with the created sensor.</returns>
        [HttpPost]
        public async Task<ActionResult> Add(SensorCreateUpdateDto sensorDto)
        {
            var validator = new SensorCreateUpdateDtoValidator();
            ValidationResult result = validator.Validate(sensorDto);

            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Details = string.Join("; ", result.Errors.Select(e => e.ErrorMessage))
                });
            }

            var sensor = SensorMapper.ToEntity(sensorDto);
            await _repository.AddAsync(sensor);

            return CreatedAtAction(nameof(GetById), new { id = sensor.Id }, SensorMapper.ToDto(sensor));
        }

        /// <summary>
        /// Updates an existing sensor.
        /// </summary>
        /// <param name="id">The sensor identifier.</param>
        /// <param name="sensorDto">The sensor data to update.</param>
        /// <returns>A 204 status if successful, or a 400 status if the identifiers do not match.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, SensorCreateUpdateDto sensorDto)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var validator = new SensorCreateUpdateDtoValidator();
            ValidationResult result = validator.Validate(sensorDto);

            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Details = string.Join("; ", result.Errors.Select(e => e.ErrorMessage))
                });
            }

            var sensor = SensorMapper.ToEntity(sensorDto);
            sensor.Id = id; // Ensure the ID is set correctly
            await _repository.UpdateAsync(sensor);

            return NoContent();
        }
    }
}