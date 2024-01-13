using FluentValidation;
using LibraryServices.Domain.DataTransferObjects.FamilyLibrary;

namespace LibraryServices.Infrastructure.Validators
{
    public class FamilyValidator : AbstractValidator<FamilyCreationDTO>
    {
        public FamilyValidator()
        {
            RuleFor(f => f.Name).NotEmpty().WithMessage("family name can not be empty");
            RuleFor(f => f.CategoryId).GreaterThan(0).WithMessage("category id can not be empty");
            RuleFor(f => f.UploaderId).GreaterThan(0).WithMessage("uploader id can not be empty");
        }
    }
}