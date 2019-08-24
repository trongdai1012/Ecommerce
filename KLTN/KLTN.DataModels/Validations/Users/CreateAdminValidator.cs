﻿using FluentValidation;
using KLTN.DataModels.Models.Users;
using System;

namespace KLTN.DataModels.Validations.Users
{
    public class CreateAdminValidator : AbstractValidator<CreateAdminViewModel>
    {
        public CreateAdminValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email không được bỏ trống");
            RuleFor(x => x.Email).MaximumLength(50).WithMessage("Độ dài tối đa 50 ký tự");
            RuleFor(x => x.Email).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email không đúng định dạng. Vd: email@example.com");
            RuleFor(x => x.PassEmail).NotEmpty().WithMessage("Password không được bỏ trống");
            RuleFor(x => x.PassEmail).MaximumLength(32).WithMessage("Độ dài tối đa 32 ký tự");
            RuleFor(x => x.ConfirmPassEmail).NotEmpty().WithMessage("ConfirmPassword không được bỏ trống");
            RuleFor(x => x.ConfirmPassEmail).MaximumLength(32).WithMessage("Độ dài tối đa 32 ký tự");
            RuleFor(x => x.Gmail).NotEmpty().WithMessage("Email không được bỏ trống");
            RuleFor(x => x.Gmail).MaximumLength(50).WithMessage("Độ dài tối đa 50 ký tự");
            RuleFor(x => x.Gmail).Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").WithMessage("Email không đúng định dạng. Vd: email@example.com");
            RuleFor(x => x.PassGmail).NotEmpty().WithMessage("Password gmail không được bỏ trống");
            RuleFor(x => x.Address).NotEmpty().WithMessage("Địa chỉ không được bỏ trống");
            RuleFor(x => x.Address).MaximumLength(100).WithMessage("Độ dài tối đa 100 ký tự");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Tên không được bỏ trống");
            RuleFor(x => x.FirstName).MaximumLength(15).WithMessage("Độ dài tối đa 15 ký tự");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Họ đệm không được bỏ trống");
            RuleFor(x => x.LastName).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
            RuleFor(x => x.BirthDay).GreaterThan(DateTime.UtcNow).WithMessage("Ngày sinh không hợp lệ");
            RuleFor(x => x.Phone).NotEmpty().WithMessage("Số điện thoại không được bỏ trống");
            RuleFor(x =>x.Phone).MaximumLength(20).WithMessage("Độ dài tối đa 20 ký tự");
        }
    }
}