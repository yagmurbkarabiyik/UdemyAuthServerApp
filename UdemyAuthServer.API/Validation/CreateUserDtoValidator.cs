using FluentValidation;
using UdemyAuthServer.Core.Dtos;

namespace UdemyAuthServer.API.Validation
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator() 
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required!").WithMessage("Email is wrong!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required!");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("User name is required!");
        }
    }
}
