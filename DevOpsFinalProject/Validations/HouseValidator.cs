using DevOpsFinalProject.Models.DataTransferObjs;
using FluentValidation;

namespace DevOpsFinalProject.Validations;

public class HouseValidator : AbstractValidator<HouseDto>
{
    public HouseValidator()
    {
        RuleFor(h => h.HostName)
            .NotEmpty()
            .WithMessage("Hostname can not be null");

        RuleFor(h => h.IsForRent)
            .Must(x => x == false || x == true)
            .WithMessage("You must mention if house is either for rent or sail");

        RuleFor(h => h.ImageUrl)
            .NotEmpty()
            .WithMessage("Image url can not be null");
    }
}
