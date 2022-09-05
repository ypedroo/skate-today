using System.Text.RegularExpressions;
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
        RuleFor(x => x.Password).Matches(new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$")).WithMessage(
            "Password must contain at least one uppercase letter, one lowercase letter, one special character, and one number");
    }
}