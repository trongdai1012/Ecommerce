using FluentValidation;
using KLTN.DataModels.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Validations.Users
{
    public class UpdateInforValidator : AbstractValidator<UpdateUserViewModel>
    {
        public UpdateInforValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được bỏ trống");
            RuleFor(x => x.Email).MaximumLength(50).WithMessage("Độ dài tối đa 50 ký tự");
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email không đúng định dạng. Vd: email@example.com");
            RuleFor(x => x.ProvinceId).NotEmpty().WithMessage("Vui lòng chọn thành phố");
            RuleFor(x => x.DistrictId).NotEmpty().WithMessage("Vui lòng chọn quận/huyện");
            RuleFor(x => x.PrecinctId).NotEmpty().WithMessage("Vui lòng chọn xã phường");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được bỏ trống");
            RuleFor(x => x.Address).MaximumLength(100).WithMessage("Độ dài tối đa 100 ký tự");
            RuleFor(x => x.BirthDay).Must(BeAValidDate).WithMessage("Ngày sinh không được bỏ trống");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được bỏ trống");
            RuleFor(x => x.FirstName).MaximumLength(15).WithMessage("Độ dài tối đa 15 ký tự");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ đệm không được bỏ trống");
            RuleFor(x => x.LastName).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Số điện thoại không được bỏ trống");
            RuleFor(x => x.Phone).MaximumLength(20).WithMessage("Độ dài tối đa 20 ký tự");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
