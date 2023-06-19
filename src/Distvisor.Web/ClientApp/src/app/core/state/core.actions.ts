import { NavigationGroup } from "./core.state";

export class ToggleNavigationSideBar {
    static readonly type = '[Core] ToggleNavigationSideBar';
    constructor(
        public readonly visible?: boolean,
    ) { }
}

export class ConfigureNavigation {
    static readonly type = '[Core] ConfigureNavigation';
    constructor(
        public readonly groups: NavigationGroup[],
    ) { }
}

export class UserSignedOut {
    static readonly type = '[Core] UserSignedOut';
}

export class UserSignedIn {
    static readonly type = '[Core] UserSignedIn';
    constructor(
        public readonly name: string,
        public readonly userName: string,
    ) { }
}

export class UserRoleReceived {
    static readonly type = '[Core] UserRoleReceived';
    constructor(
        public readonly role?: string
    ) { }
}