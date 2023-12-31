/* tslint:disable */
/* eslint-disable */
import { RedirectionDto } from './redirection-dto';
export interface RedirectionsListDto {
  first?: number;
  items?: null | Array<RedirectionDto>;
  rows?: number;
  totalRecords?: number;
}
