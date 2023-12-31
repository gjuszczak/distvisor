/* tslint:disable */
/* eslint-disable */
import { GatewaySessionDto } from './gateway-session-dto';
export interface GatewaySessionsListDto {
  first?: number;
  items?: null | Array<GatewaySessionDto>;
  rows?: number;
  totalRecords?: number;
}
