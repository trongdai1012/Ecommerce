using FluentValidation;
using KLTN.DataModels.Models.Users;

namespace KLTN.DataModels.Validations.Users
{
    public class UpdateAdminValidator : AbstractValidator<UpdateAdminViewModel>
    {
        public UpdateAdminValidator()
        {
            RuleFor(x=>x.);
        }
    }
}
