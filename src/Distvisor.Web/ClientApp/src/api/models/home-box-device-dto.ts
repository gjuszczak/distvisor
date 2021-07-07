/* tslint:disable */
/* eslint-disable */
import { HomeBoxDeviceType } from './home-box-device-type';
export interface HomeBoxDeviceDto {
  header?: null | string;
  id?: null | string;
  location?: null | string;
  name?: null | string;
  online?: boolean;
  params?: any;
  type?: HomeBoxDeviceType;
}
