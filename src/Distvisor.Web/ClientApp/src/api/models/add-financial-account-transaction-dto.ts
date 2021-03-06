/* tslint:disable */
/* eslint-disable */
import { FinancialAccountTransactionSource } from './financial-account-transaction-source';
export interface AddFinancialAccountTransactionDto {
  accountId?: string;
  amount?: number;
  balance?: number;
  id?: string;
  postingDate?: string;
  seqNo?: number;
  source?: FinancialAccountTransactionSource;
  title?: null | string;
  transactionDate?: string;
  transactionHash?: null | string;
}
