using ChatInRealTime.Core.Dtos.Users;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatInRealTime.Core.Validation.User
{
    public class SignUpUserValidation : AbstractValidator<SignUpUserDto>
    {
        public SignUpUserValidation()
        {
            RuleFor(x => x.Name)
              .NotNull().NotEmpty().WithMessage("Name cannot be empty")
              .MaximumLength(25).WithMessage("Maximum name length is 25 characters")
              .MinimumLength(2).WithMessage("Minimum name length is 2 characters");

            RuleFor(x => x.Surname)
              .NotNull().NotEmpty().WithMessage("Surname cannot be empty")
              .MaximumLength(30).WithMessage("Maximum surname length is 30 characters")
              .MinimumLength(4).WithMessage("Minimum surname length is 4 characters");

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

            RuleFor(x => x.ConfirmPassword)
              .Equal(x => x.Password).WithMessage("Must match the password");
        }
    }
}
