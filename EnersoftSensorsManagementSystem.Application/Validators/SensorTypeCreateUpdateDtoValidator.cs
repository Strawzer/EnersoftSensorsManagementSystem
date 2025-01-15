using EnersoftSensorsManagementSystem.Application.DTOs;
using FluentValidation;

namespace EnersoftSensorsManagementSystem.Application.Validators;

public class SensorTypeCreateUpdateDtoValidator : AbstractValidator<SensorTypeCreateUpdateDto>
{
    public SensorTypeCreateUpdateDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Sensor type name is required.")
            .MaximumLength(50).WithMessage("Sensor type name cannot exceed 50 characters.");
    }
}
