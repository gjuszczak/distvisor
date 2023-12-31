import { BackupFilesListDto } from "src/app/api/models";
import { BackupModals } from "./backups.state";

export class LoadBackups {
    static readonly type = '[Admin] LoadBackups';
    constructor(
        public readonly first?: number,
        public readonly rows?: number
    ) { }
}

export class LoadBackupsSuccess {
    static readonly type = '[Admin] LoadBackupsSuccess';
    constructor(
        public readonly backups: BackupFilesListDto 
    ) { }
}

export class LoadBackupsFail {
    static readonly type = '[Admin] LoadBackupsFail';
    constructor(
        public readonly error: string 
    ) { }
}

export class ShowBackupModal {
    static readonly type = '[Admin] ShowBackupModal';
    constructor ( 
        public readonly modal: BackupModals,
        public readonly defaultInputText: string = ''
    ) { }
}

export class HideBackupModal {
    static readonly type = '[Admin] HideBackupModal';
    constructor (
        public readonly reloadBackups: boolean = false
    ) { }
}

export class FailBackupModal {
    static readonly type = '[Admin] FailBackupModal';
    constructor (
        public readonly error: string
    ) { }
}

export class CreateBackup {
    static readonly type = '[Admin] CreateBackup';
    constructor(
        public readonly name: string 
    ) { }
}

export class RenameBackup {
    static readonly type = '[Admin] RenameBackup';
    constructor(
        public readonly oldName: string,
        public readonly newName: string
    ) { }
}

export class DeleteBackup {
    static readonly type = '[Admin] DeleteBackup';
    constructor(
        public readonly name: string 
    ) { }
}

export class RestoreBackup {
    static readonly type = '[Admin] RestoreBackup';
    constructor(
        public readonly name: string 
    ) { }
}