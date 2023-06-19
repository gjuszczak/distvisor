/* tslint:disable */
/* eslint-disable */
import { GatewaySessionStatus } from './gateway-session-status';
export interface GatewaySessionDto {
  id?: string;
  status?: GatewaySessionStatus;
  tokenGeneratedAt?: string;
}
