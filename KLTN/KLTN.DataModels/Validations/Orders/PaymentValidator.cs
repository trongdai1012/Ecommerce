using FluentValidation;
using KLTN.DataModels.Models.Orders;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Validations.Orders
{
    public class PaymentValidator : AbstractValidator<OrderViewModel>
    {
        public PaymentValidator()
        {
            RuleFor(x => x.RecipientPhone).NotEmpty().WithMessage("Số điện thoại không được bỏ trống");
            RuleFor(x => x.RecipientAddress).NotEmpty().WithMessage("Địa chỉ không được bỏ trống");
            RuleFor(x => x.RecipientProvinceCode).NotEmpty().WithMessage("Vui lòng chọn thành phố");
            RuleFor(x => x.RecipientDistrictCode).NotEmpty().WithMessage("Vui lòng chọn quận/huyện");
            RuleFor(x => x.RecipientPrecinctCode).NotEmpty().WithMessage("Vui lòng chọn xã/phường");
            RuleFor(x => x.RecipientFirstName).NotEmpty().WithMessage("Vui lòng nhập tên");
            RuleFor(x => x.RecipientLastName).NotEmpty().WithMessage("Vui lòng nhập họ đệm");
            RuleFor(x => x.RecipientEmail).NotEmpty().WithMessage("Vui lòng nhập email");
        }
    }
}
