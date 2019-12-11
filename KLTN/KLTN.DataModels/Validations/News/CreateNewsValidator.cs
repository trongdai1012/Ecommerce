﻿using FluentValidation;
using KLTN.DataModels.Models.News;
using System;
using System.Collections.Generic;
using System.Text;

namespace KLTN.DataModels.Validations.News
{
    public class CreateNewsValidator: AbstractValidator<NewsViewModel>
    {
        public CreateNewsValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Tiêu đề không được bỏ trống.");
            RuleFor(x => x.Title).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
            RuleFor(x => x.Title).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
            RuleFor(x => x.Content).NotEmpty().WithMessage("Nội dung không được bỏ trống.");
            RuleFor(x => x.Content).MinimumLength(6).WithMessage("Độ dài tối thiểu 6 ký tự.");
            RuleFor(x => x.Content).MaximumLength(30).WithMessage("Độ dài tối đa 30 ký tự");
        }
    }
}
