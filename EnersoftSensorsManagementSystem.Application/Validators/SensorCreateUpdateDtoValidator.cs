using EnersoftSensorsManagementSystem.Application.DTOs;
using FluentValidation;

namespace EnersoftSensorsManagementSystem.Application.Validators;

public class SensorCreateUpdateDtoValidator : AbstractValidator<SensorCreateUpdateDto>
{
    public SensorCreateUpdateDtoValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("Sensor name is required.")
            .MaximumLength(100).WithMessage("Sensor name cannot exceed 100 characters.");

        RuleFor(s => s.Location)
            .NotEmpty().WithMessage("Location is required.")
            .MaximumLength(200).WithMessage("Location cannot exceed 200 characters.");

        RuleFor(s => s.TypeId)
            .GreaterThan(0).WithMessage("TypeId must be a valid positive integer.");
    }
}
