using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using WebApi.Features.Accounts.Models;
using WebApi.Domain.Extensions;
using Infrastructure.Password;

namespace WebApi.Features.Accounts.Validations
{
    public class RegisterUserModelValidation : AbstractValidator<RegisterUserModel>
    {
        public RegisterUserModelValidation(IPasswordComplexity complexity)
        {
            RuleFor(x => x.EmailAddress)
                .EmailAddress().WithMessage("Please enter a valid email address")
                .NotEmpty().WithMessage("Please enter an email address")
                .Length(0, 255);

            RuleFor(x => x.Password)
                .Rule(complexity.Validate)
                .WithMessage(complexity.ComplexityMessage);
        }
    }
}
