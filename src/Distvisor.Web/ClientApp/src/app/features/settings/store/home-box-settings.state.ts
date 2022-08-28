import { Injectable } from '@angular/core';
import { State } from '@ngxs/store';

import { GatewaySessionsState } from './gateway-sessions.state';

export interface HomeBoxSettingsStateModel {
}

export const homeBoxSettingsStateDefaults: HomeBoxSettingsStateModel = {
};

@State<HomeBoxSettingsStateModel>({
    name: 'homeBoxSettings',
    defaults: homeBoxSettingsStateDefaults,
    children: [GatewaySessionsState],
})
@Injectable()
export class HomeBoxSettingsState { }