using FluentValidation;

namespace Distvisor.App.Features.HomeBox.Commands.OpenGatewaySession
{
    public class OpenGatewaySessionValidator : AbstractValidator<OpenGatewaySession>
    {
        public OpenGatewaySessionValidator()
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
