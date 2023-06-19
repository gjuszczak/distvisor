import { GatewaySessionsListDto } from 'src/app/api/models';

export class LoadGatewaySessions {
    static readonly type = '[Home Box] LoadGatewaySessions';
    constructor(
        public readonly first?: number,
        public readonly rows?: number
    ) { }
}

export class LoadGatewaySessionsSuccess {
    static readonly type = '[Home Box] LoadGatewaySessionsSuccess';
    constructor(
        public readonly gatewaySessions: GatewaySessionsListDto
    ) { }
}

export class LoadGatewaySessionsFail {
    static readonly type = '[Admin] LoadGatewaySessionsFail';
    constructor(
        public readonly error: string
    ) { }
}

export class OpenGatewaySession {
    static readonly type = '[Home Box] OpenGatewaySession';
    constructor(
        public readonly user: string,
        public readonly password: string
    ) { }
}

export class RefreshGatewaySession {
    static readonly type = '[Home Box] RefreshGatewaySession';
    constructor(
        public readonly sessionId: string
    ) { }
}

export class DeleteGatewaySession {
    static readonly type = '[Home Box] DeleteGatewaySession';
    constructor(
        public readonly sessionId: string
    ) { }
}