import { Injectable } from '@angular/core';
import { Action, Selector, State, StateContext } from '@ngxs/store';
import { ConfigureNavigation, ToggleNavigationSideBar, UserRoleReceived, UserSignedIn, UserSignedOut } from './core.actions';
import { patch } from '@ngxs/store/operators';
import { UserService } from 'src/app/api/services';
import { catchError, mergeMap } from 'rxjs';

export interface NavigationItem {
    label: string;
    icon: string;
    route: string;
    routeExact: boolean;
    minimalRole?: UserRole;
}

export interface NavigationGroup {
    label: string;
    items: NavigationItem[];
}

export interface NavigationStateModel {
    sideBarVisible: boolean,
    groups: NavigationGroup[]
}

export enum UserRole {
    None,
    Guest,
    User,
    Admin
};

export interface UserStateModel {
    isAuthenticated: boolean,
    name?: string,
    username?: string,
    role?: UserRole,
}

export interface CoreStateModel {
    navigation: NavigationStateModel,
    user: UserStateModel,
}

export const coreStateDefaults: CoreStateModel = {
    navigation: {
        sideBarVisible: false,
        groups: []
    },
    user: {
        isAuthenticated: false
    }
};

@State<CoreStateModel>({
    name: 'core',
    defaults: coreStateDefaults,
})
@Injectable({
    providedIn: 'root'
})
export class CoreState {

    constructor(
        private userService: UserService
    ) { }

    @Selector()
    static isNavigationSideBarVisible(state: CoreStateModel) {
        return state.navigation.sideBarVisible;
    }

    @Selector()
    static userRole(state: CoreStateModel) {
        return state.user.role;
    }

    @Selector([CoreState, CoreState.userRole])
    static userNavigationGroups(state: CoreStateModel, userRole?: UserRole) {
        let userNavigationGroups = structuredClone(state.navigation.groups);
        userNavigationGroups.forEach(group => {
            group.items = group.items.filter(item => (item.minimalRole ?? -1) <= (userRole ?? -1));
        })
        userNavigationGroups = userNavigationGroups.filter(group => group.items.length > 0);
        return userNavigationGroups;
    }

    @Action(ToggleNavigationSideBar)
    toggleNavigationSideBar(ctx: StateContext<CoreStateModel>, { visible }: ToggleNavigationSideBar) {
        const state = ctx.getState();
        if (visible === undefined) {
            visible = !state.navigation.sideBarVisible;
        }
        if (state.navigation.sideBarVisible !== visible) {
            ctx.setState(this.setNavigationSideBarVisibility(visible));
        }
    }

    @Action(ConfigureNavigation)
    configureNavigation(ctx: StateContext<CoreStateModel>, { groups }: ConfigureNavigation) {
        ctx.setState(this.setNavigationGroups(groups));
    }

    @Action(UserSignedOut)
    userSignedOut(ctx: StateContext<CoreStateModel>) {
        ctx.setState(this.setUser(false));
        return ctx.dispatch(new ToggleNavigationSideBar(false));
    }

    @Action(UserSignedIn)
    userSignedIn(ctx: StateContext<CoreStateModel>, { name, userName }: UserSignedIn) {
        ctx.setState(this.setUser(true, name, userName, UserRole.None));

        return this.userService.apiUserGet$Json().pipe(
            mergeMap(user => ctx.dispatch(new UserRoleReceived(user.role ? user.role : undefined))),
            catchError(e => {
                console.error(`Unable to get user roles. ${e}`);
                return ctx.dispatch(new UserRoleReceived());
            })
        );
    }

    @Action(UserRoleReceived)
    userRoleReceived(ctx: StateContext<CoreStateModel>, { role }: UserRoleReceived) {
        let typedRole = UserRole.Guest;
        switch (role?.toLowerCase()) {
            case 'user':
                typedRole = UserRole.User;
                break;
            case 'admin':
                typedRole = UserRole.Admin;
                break;
        };
        ctx.setState(this.setUserRole(typedRole));

        return ctx.dispatch(new ToggleNavigationSideBar(true));
    }

    private setNavigationSideBarVisibility(visibility: boolean) {
        return patch<CoreStateModel>({
            navigation: patch<NavigationStateModel>({
                sideBarVisible: visibility,
            })
        });
    }

    private setNavigationGroups(groups: NavigationGroup[]) {
        return patch<CoreStateModel>({
            navigation: patch<NavigationStateModel>({
                groups: groups,
            })
        });
    }

    private setUser(isAuthenticated: boolean, name?: string, username?: string, role?: UserRole) {
        return patch<CoreStateModel>({
            user: {
                isAuthenticated,
                name,
                username,
                role
            }
        });
    }

    private setUserRole(role: UserRole) {
        return patch<CoreStateModel>({
            user: patch<UserStateModel>({
                role
            })
        });
    }
}