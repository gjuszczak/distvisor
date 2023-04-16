import { Injectable } from "@angular/core";
import { State } from "@ngxs/store";
import { BackupsState } from "./backups.state";

export interface AdminStateModel {
};

export const adminStateDefaults: AdminStateModel = {
};

@State<AdminStateModel>({
    name: 'admin',
    defaults: adminStateDefaults,
    children: [BackupsState]
})
@Injectable()
export class AdminState {
}