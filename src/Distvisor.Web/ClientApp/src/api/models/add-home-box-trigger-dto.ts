/* tslint:disable */
/* eslint-disable */
import { HomeBoxTriggerAction } from './home-box-trigger-action';
import { HomeBoxTriggerSource } from './home-box-trigger-source';
import { HomeBoxTriggerTarget } from './home-box-trigger-target';
export interface AddHomeBoxTriggerDto {
  actions?: null | Array<HomeBoxTriggerAction>;
  enabled?: boolean;
  id?: string;
  sources?: null | Array<HomeBoxTriggerSource>;
  targets?: null | Array<HomeBoxTriggerTarget>;
}
