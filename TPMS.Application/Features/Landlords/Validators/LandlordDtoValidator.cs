using FluentValidation;
using TPMS.Application.Features.Landlords.DTOs;


namespace TPMS.Application.Features.Landlords.Validators
{
    public class LandlordDtoValidator : AbstractValidator<LandlordDto>
    {
        public LandlordDtoValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
