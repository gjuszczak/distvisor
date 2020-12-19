/* tslint:disable */
/* eslint-disable */
import { FinancialAccountType } from './financial-account-type';
export interface FinancialAccountDto {
  id?: null | string;
  name?: null | string;
  number?: null | string;
  paycards?: null | Array<string>;
  type?: FinancialAccountType;
}
