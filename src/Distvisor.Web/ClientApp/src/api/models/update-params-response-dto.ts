/* tslint:disable */
import { DbUpdateStrategy } from './db-update-strategy';
export interface UpdateParamsResponseDto {
  dbUpdateStrategies?: null | Array<DbUpdateStrategy>;
  versions?: null | Array<string>;
}
