/* tslint:disable */
/* eslint-disable */
import { BackupFileDto } from './backup-file-dto';
export interface BackupFilesListDto {
  first?: number;
  items?: null | Array<BackupFileDto>;
  rows?: number;
  totalRecords?: number;
}
