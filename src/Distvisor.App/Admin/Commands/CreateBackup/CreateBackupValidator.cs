using FluentValidation;

namespace Distvisor.App.Admin.Commands.CreateBackup
{
    public class CreateBackupValidator : AbstractValidator<CreateBackup>
    {
        public CreateBackupValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
