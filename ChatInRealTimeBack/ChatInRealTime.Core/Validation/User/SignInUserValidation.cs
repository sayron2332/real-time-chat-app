using ChatInRealTime.Core.Dtos.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Validation.User
{
    public class SignInUserValidation : AbstractValidator<SignInUserDto>
    {
        public SignInUserValidation()
        {
            RuleFor(x => x.Email)
             .NotNull().NotEmpty().WithMessage("Email cannot be empty")
             .MaximumLength(128).WithMessage("Maximum email length is 128 characters")
             .EmailAddress().WithMessage("Email is invalid");

            RuleFor(x => x.Password)
              .NotNull().NotEmpty().WithMessage("Password cannot be empty")
              .MinimumLength(8).WithMessage("Minimum password length is 8 characters")
              .MaximumLength(16).WithMessage("Maximum password length is 16 characters")
              .Matches(@"[A-Z]+").WithMessage("Password must contain at least 1 uppercase letter")
              .Matches(@"[a-z]+").WithMessage("Password must contain at least 1 lowercase letter")
              .Matches(@"[0-9]+").WithMessage("Password must contain at least 1 digit");
        }
    }
}
