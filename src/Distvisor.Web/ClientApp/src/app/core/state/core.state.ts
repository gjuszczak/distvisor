import { Injectable } from '@angular/core';
import { State } from '@ngxs/store';

export interface CoreStateModel {
}

@State<CoreStateModel>({
    name: 'core',
    defaults: {},
})
@Injectable({
    providedIn: 'root'
})
export class CoreState { }