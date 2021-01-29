/* tslint:disable */
/* eslint-disable */
import { FinancialSummaryDataSetDto } from './financial-summary-data-set-dto';
export interface FinancialSummaryLineChartDto {
  labels?: null | Array<string>;
  separateDataSets?: null | Array<FinancialSummaryDataSetDto>;
  summaryDataSet?: FinancialSummaryDataSetDto;
}
