using FluentValidation;

namespace Distvisor.App.Admin.Commands.RenameBackup
{
    public class RenameBackupValidator : AbstractValidator<RenameBackup>
    {
        public RenameBackupValidator()
        {
            RuleFor(v => v.OldName)
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(v => v.NewName)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
