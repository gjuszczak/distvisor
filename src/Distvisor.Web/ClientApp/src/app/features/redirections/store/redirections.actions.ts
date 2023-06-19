import { RedirectionsListDto } from "src/app/api/models";
import { Redirection, RedirectionModalKind } from "./redirections.state";

export class LoadRedirections {
    static readonly type = '[Admin] LoadRedirections';
    constructor(
        public readonly first?: number,
        public readonly rows?: number
    ) { }
}

export class LoadRedirectionsSuccess {
    static readonly type = '[Admin] LoadRedirectionsSuccess';
    constructor(
        public readonly redirections: RedirectionsListDto 
    ) { }
}

export class LoadRedirectionsFail {
    static readonly type = '[Admin] LoadRedirectionsFail';
    constructor(
        public readonly error: string 
    ) { }
}

export class ShowRedirectionModal {
    static readonly type = '[Admin] ShowRedirectionModal';
    constructor ( 
        public readonly modal: RedirectionModalKind,
        public readonly value: Redirection,
    ) { }
}

export class HideRedirectionModal {
    static readonly type = '[Admin] HideRedirectionModal';
    constructor (
        public readonly reloadRedirections: boolean = false
    ) { }
}

export class FailRedirectionModal {
    static readonly type = '[Admin] FailRedirectionModal';
    constructor (
        public readonly error: string
    ) { }
}

export class CreateRedirection {
    static readonly type = '[Admin] CreateRedirection';
    constructor(
        public readonly name: string,
        public readonly url: string,
    ) { }
}

export class EditRedirection {
    static readonly type = '[Admin] EditRedirection';
    constructor(
        public readonly redirectionId: string,
        public readonly name: string,
        public readonly url: string,
    ) { }
}

export class DeleteRedirection {
    static readonly type = '[Admin] DeleteRedirection';
    constructor(
        public readonly redirectionId: string,
    ) { }
}