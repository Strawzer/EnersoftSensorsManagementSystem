using EnersoftSensorsManagementSystem.API.Models;
using EnersoftSensorsManagementSystem.Application.DTOs;
using EnersoftSensorsManagementSystem.Application.Mappers;
using EnersoftSensorsManagementSystem.Application.Validators;
using EnersoftSensorsManagementSystem.Core.Interfaces;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EnersoftSensorsManagementSystem.API.Controllers
{
    /// <summary>
    /// API controller for managing sensor types.
    /// </summary>
    [ApiController]
    [Route(("api/v{version:apiVersion}/[controller]"))]
    [Authorize(Roles = "Admin")]
    public class SensorTypesController : ControllerBase
    {
        private readonly ISensorTypeRepository _repository;
        private readonly IMemoryCache _cache;
        private const string SensorTypesCacheKey = "GetAllSensorTypes";

        public SensorTypesController(ISensorTypeRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all sensor types.
        /// </summary>
        /// <returns>A collection of SensorTypeDto.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SensorTypeDto>>> GetAll()
        {
            if (!_cache.TryGetValue(SensorTypesCacheKey, out IEnumerable<SensorTypeDto> sensorTypes))
            {
                var sensorTypeEntities = await _repository.GetAllAsync();
                sensorTypes = sensorTypeEntities.Select(SensorTypeMapper.ToDto);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                    .SetAbsoluteExpiration(TimeSpan.FromHours(1));

                _cache.Set(SensorTypesCacheKey, sensorTypes, cacheOptions);
            }

            return Ok(sensorTypes);
        }

        /// <summary>
        /// Retrieves a sensor type by its identifier.
        /// </summary>
        /// <param name="id">The sensor type identifier.</param>
        /// <returns>The sensor type with the specified identifier, or a 404 status if not found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SensorTypeDto>> GetById(int id)
        {
            var sensorType = await _repository.GetByIdAsync(id);
            if (sensorType == null) return NotFound();

            return Ok(SensorTypeMapper.ToDto(sensorType));
        }

        /// <summary>
        /// Adds a new sensor type.
        /// </summary>
        /// <param name="sensorTypeDto">The sensor type data to add.</param>
        /// <returns>A 201 status with the created sensor type.</returns>
        [HttpPost]
        public async Task<ActionResult> Add(SensorTypeCreateUpdateDto sensorTypeDto)
        {
            var validator = new SensorTypeCreateUpdateDtoValidator();
            ValidationResult result = validator.Validate(sensorTypeDto);

            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Details = string.Join("; ", result.Errors.Select(e => e.ErrorMessage))
                });
            }

            var sensorType = SensorTypeMapper.ToEntity(sensorTypeDto);
            await _repository.AddAsync(sensorType);

            return CreatedAtAction(nameof(GetById), new { id = sensorType.Id }, SensorTypeMapper.ToDto(sensorType));
        }

        /// <summary>
        /// Updates an existing sensor type.
        /// </summary>
        /// <param name="id">The sensor type identifier.</param>
        /// <param name="sensorTypeDto">The sensor type data to update.</param>
        /// <returns>A 204 status if successful, or a 400 status if the identifiers do not match.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, SensorTypeCreateUpdateDto sensorTypeDto)
        {
            if (id <= 0) return BadRequest("Invalid ID.");

            var validator = new SensorTypeCreateUpdateDtoValidator();
            ValidationResult result = validator.Validate(sensorTypeDto);

            if (!result.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    StatusCode = 400,
                    Message = "Validation failed",
                    Details = string.Join("; ", result.Errors.Select(e => e.ErrorMessage))
                });
            }

            var sensorType = SensorTypeMapper.ToEntity(sensorTypeDto);
            sensorType.Id = id; // Ensure the ID is set correctly
            await _repository.UpdateAsync(sensorType);

            return NoContent();
        }

        /// <summary>
        /// Deletes a sensor type by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the sensor type to delete.</param>
        /// <returns>A 204 status if successful.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);

            // Remove cached data when a sensor type is deleted
            _cache.Remove(SensorTypesCacheKey);

            return NoContent();
        }
    }
}
