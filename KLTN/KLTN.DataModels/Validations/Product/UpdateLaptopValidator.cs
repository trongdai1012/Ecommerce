using FluentValidation;
using KLTN.DataModels.Models.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Validations.Product
{
    public class UpdateLaptopValidator : AbstractValidator<UpdateLaptopViewModel>
    {
        public UpdateLaptopValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên laptop không được bỏ trống.");
            RuleFor(x => x.Name).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
            RuleFor(x => x.Name).MaximumLength(100).WithMessage("Độ dài tối đa 100 ký tự");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Miêu tả sản phẩm không được bỏ trống.");
            RuleFor(x => x.Description).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
        }
    }
}
