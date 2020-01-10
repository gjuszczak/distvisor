/* tslint:disable */
import { DbUpdateStrategy } from './db-update-strategy';
export interface UpdateRequestDto {
  dbUpdateStrategy?: DbUpdateStrategy;
  updateToVersion?: null | string;
}
