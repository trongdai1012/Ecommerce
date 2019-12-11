using FluentValidation;
using KLTN.DataModels.Models.Brands;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Validations.Brands
{
    public class CreateBrandValidator : AbstractValidator<CreateBrandModel>
    {
        public CreateBrandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên thương hiệu không được bỏ trống.");
            RuleFor(x => x.Name).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
            RuleFor(x => x.Name).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ thương hiệu không được bỏ trống.");
            RuleFor(x => x.Address).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
            RuleFor(x => x.Address).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
        }
    }
}
