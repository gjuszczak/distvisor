/* tslint:disable */
/* eslint-disable */
import { GatewaySessionDto } from './gateway-session-dto';
export interface GatewaySessionDtoPaginatedList {
  firstOffset?: number;
  items?: null | Array<GatewaySessionDto>;
  pageSize?: number;
  totalCount?: number;
}
