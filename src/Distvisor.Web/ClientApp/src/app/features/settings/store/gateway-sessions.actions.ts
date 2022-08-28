import {
    GatewaySessionDtoPaginatedList,
    LoginToGateway as LoginToGatewayCommand,
    RefreshGatewaySession as RefreshGatewaySessionCommand,
    DeleteGatewaySession as DeleteGatewaySessionCommand,
} from 'src/app/api/models';

export class LoadGatewaySessions {
    static readonly type = '[Home Box Settings] Load Gateway Sessions';
    constructor(
        public readonly firstOffset?: number,
        public readonly pageSize?: number
    ) { }
}

export class LoadGatewaySessionsSuccess {
    static readonly type = '[Home Box Settings] Load Gateway Sessions Success';
    constructor(
        public readonly gatewaySessions: GatewaySessionDtoPaginatedList
    ) { }
}

export class LoginToGateway {
    static readonly type = '[Home Box Settings] Login To Gateway';
    constructor(
        public readonly command: LoginToGatewayCommand
    ) { }
}

export class RefreshGatewaySession {
    static readonly type = '[Home Box Settings] Refresh Gateway Session';
    constructor(
        public readonly command: RefreshGatewaySessionCommand
    ) { }
}

export class DeleteGatewaySession {
    static readonly type = '[Home Box Settings] Delete Gateway Session';
    constructor(
        public readonly command: DeleteGatewaySessionCommand
    ) { }
}