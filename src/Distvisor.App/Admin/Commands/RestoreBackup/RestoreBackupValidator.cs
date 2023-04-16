using FluentValidation;

namespace Distvisor.App.Admin.Commands.RestoreBackup
{
    public class RestoreBackupValidator : AbstractValidator<RestoreBackup>
    {
        public RestoreBackupValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
