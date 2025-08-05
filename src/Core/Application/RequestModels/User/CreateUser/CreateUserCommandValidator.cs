using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace Application.RequestModels.User.CreateUser
{
    public class CreateUserCommandValidator:AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(i => i.FullName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .MaximumLength(100)
                .WithMessage("{PropertyName} must be at most 100 characters");

            RuleFor(i => i.Email)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .EmailAddress(FluentValidation.Validators.EmailValidationMode.AspNetCoreCompatible)
                .WithMessage("{PropertyName} is not a valid email address");

            RuleFor(i => i.firstName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must be at most 50 characters");

            RuleFor(i => i.lastName)
                .NotEmpty()
                .WithMessage("{PropertyName} cannot be empty")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must be at most 50 characters");

            RuleFor(i => i.PasswordHash)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .MinimumLength(6)
                .WithMessage("{PropertyName} should be at least six characters");

            RuleFor(i => i.PhoneNumber)
                .NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Matches(@"^\+?[1-9][0-9]{7,14}$") // Uluslararası telefon numarası formatı
                .WithMessage("{PropertyName} is not a valid phone number");

           
        }
    }
}
