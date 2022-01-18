using FluentValidation;

namespace Distvisor.App.HomeBox.Commands.LoginToGateway
{
    public class LoginToGatewayValidatior : AbstractValidator<LoginToGateway>
    {
        public LoginToGatewayValidatior()
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
