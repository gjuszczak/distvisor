/* tslint:disable */
/* eslint-disable */
import { GatewaySessionDto } from './gateway-session-dto';
export interface GatewaySessionDtoPaginatedList {
  first?: number;
  items?: null | Array<GatewaySessionDto>;
  rows?: number;
  totalRecords?: number;
}
