/* tslint:disable */
/* eslint-disable */
import { DeviceType } from './device-type';
export interface DeviceDto {
  gatewayDeviceId?: null | string;
  header?: null | string;
  id?: string;
  isOnline?: boolean;
  location?: null | string;
  name?: null | string;
  params?: any;
  type?: DeviceType;
}
