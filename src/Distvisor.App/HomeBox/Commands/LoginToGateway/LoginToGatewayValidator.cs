using FluentValidation;

namespace Distvisor.App.HomeBox.Commands.LoginToGateway
{
    public class LoginToGatewayValidator : AbstractValidator<LoginToGateway>
    {
        public LoginToGatewayValidator()
        {
            RuleFor(v => v.User)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(v => v.Password)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
