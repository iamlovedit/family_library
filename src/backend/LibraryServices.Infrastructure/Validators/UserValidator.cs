using FluentValidation;
using LibraryServices.Domain.Models.Identity;

namespace LibraryServices.Infrastructure.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(u => u.Username).NotEmpty().WithMessage("username can not be empty");
            RuleFor(u => u.Email).EmailAddress().WithMessage("email is not valid");
            RuleFor(u => u.Password).NotEmpty().WithMessage("password can not be empty");
        }
    }
}