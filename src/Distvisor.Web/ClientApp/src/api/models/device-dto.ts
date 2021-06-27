/* tslint:disable */
/* eslint-disable */
import { DeviceTypeDto } from './device-type-dto';
export interface DeviceDto {
  identifier?: null | string;
  location?: null | string;
  name?: null | string;
  online?: boolean;
  params?: any;
  type?: DeviceTypeDto;
}
