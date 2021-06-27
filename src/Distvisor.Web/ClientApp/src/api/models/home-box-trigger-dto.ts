/* tslint:disable */
/* eslint-disable */
import { HomeBoxTriggerAction } from './home-box-trigger-action';
import { HomeBoxTriggerExecutionMemory } from './home-box-trigger-execution-memory';
import { HomeBoxTriggerSource } from './home-box-trigger-source';
import { HomeBoxTriggerTarget } from './home-box-trigger-target';
export interface HomeBoxTriggerDto {
  actions?: null | Array<HomeBoxTriggerAction>;
  enabled?: boolean;
  executionMemory?: HomeBoxTriggerExecutionMemory;
  id?: string;
  sources?: null | Array<HomeBoxTriggerSource>;
  targets?: null | Array<HomeBoxTriggerTarget>;
}
