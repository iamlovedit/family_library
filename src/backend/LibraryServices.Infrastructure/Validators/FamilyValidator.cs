using FluentValidation;
using LibraryServices.Domain.Models.FamilyLibrary;

namespace LibraryServices.Infrastructure.Validators
{
    public class FamilyValidator : AbstractValidator<Family>
    {
        public FamilyValidator()
        {
            RuleFor(f => f.Name).NotEmpty().WithMessage("family name can not be empty");
            RuleFor(f => f.CategoryId).GreaterThan(0).WithMessage("category id can not be empty");
        }
    }
}