/* tslint:disable */
/* eslint-disable */
import { DeviceDto } from './device-dto';
export interface DevicesListDto {
  first?: number;
  items?: null | Array<DeviceDto>;
  rows?: number;
  totalRecords?: number;
}
