﻿using FluentValidation;

namespace Distvisor.App.Admin.Commands.DeleteBackup
{
    public class DeleteBackupValidator : AbstractValidator<DeleteBackup>
    {
        public DeleteBackupValidator()
        {
            RuleFor(v => v.Name)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
