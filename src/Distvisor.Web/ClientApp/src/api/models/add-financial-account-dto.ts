/* tslint:disable */
/* eslint-disable */
import { FinancialAccountType } from './financial-account-type';
export interface AddFinancialAccountDto {
  createdDateTimeUtc?: string;
  id?: string;
  name?: null | string;
  number?: null | string;
  type?: FinancialAccountType;
}
