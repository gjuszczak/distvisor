import { Injectable } from '@angular/core';
import { State } from '@ngxs/store';

import { GatewaySessionsState } from './gateway-sessions.state';
import { DevicesState } from './devices.state';

export interface HomeBoxStateModel {
}

export const homeBoxStateDefaults: HomeBoxStateModel = {
};

@State<HomeBoxStateModel>({
    name: 'homeBox',
    defaults: homeBoxStateDefaults,
    children: [GatewaySessionsState, DevicesState],
})
@Injectable()
export class HomeBoxState { }