using System;
using FluentValidation;

namespace API.DTOs;

public class CreateServiceScheduleDtoValidator : AbstractValidator<CreateServiceScheduleDto>
{
    public CreateServiceScheduleDtoValidator()
    {
        RuleFor(x => x.ServiceName).NotEmpty().WithMessage("Service name is required");
        RuleFor(x => x.ServiceId).GreaterThan(0).WithMessage("ServiceId must be valid");
        RuleFor(x => x.SessionCount).GreaterThan(0).WithMessage("Session count must be greater than 0.");
        RuleFor(x => x.IntervalInDays).GreaterThan(0).WithMessage("Interval must be greater than 0.");
    }
}
