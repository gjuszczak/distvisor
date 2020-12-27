/* tslint:disable */
/* eslint-disable */
import { FinancialAccountTransactionDataSource } from './financial-account-transaction-data-source';
export interface FinancialAccountTransactionDto {
  accountId?: string;
  amount?: number;
  balance?: number;
  dataSource?: FinancialAccountTransactionDataSource;
  date?: string;
  details?: null | string;
  id?: string;
  isBalanceEstimated?: boolean;
}
