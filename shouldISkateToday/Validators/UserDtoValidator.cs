using FluentValidation;
using shouldISkateToday.Domain.Dtos;

namespace shouldISkateToday.Validators;

public class UserDtoValidator : AbstractValidator<UserDto>
{
    public UserDtoValidator()
    {
        RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Name cannot be Null or Empty");
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(16)
            .WithMessage("Password must be between 8 and 16 characters");
    }
}