using Distvisor.App.Features.Common.Extensions;
using FluentValidation;

namespace Distvisor.App.Features.Redirections.Commands
{
    public abstract class RedirectionBaseValidator<T> : AbstractValidator<T>
        where T : RedirectionBaseCommand
    {
        protected RedirectionBaseValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(v => v.Url)
                .MaximumLength(2000)
                .NotEmpty()
                .IsValidUri();
        }
    }
}
