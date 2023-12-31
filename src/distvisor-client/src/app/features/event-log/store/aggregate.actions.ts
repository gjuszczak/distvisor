import { AggregateDto} from "src/app/api/models";

export class LoadAggregate {
    static readonly type = '[Event Log] LoadAggregate';
    constructor(
        public readonly aggregateId: string,
    ) { }
}

export class LoadAggregateSuccess {
    static readonly type = '[Event Log] LoadAggregateSuccess';
    constructor(
        public readonly aggregate: AggregateDto 
    ) { }
}

export class LoadAggregateFail {
    static readonly type = '[Event Log] LoadAggregateFail';
    constructor(
        public readonly error: any 
    ) { }
}

export class ClearAggregate {
    static readonly type = '[Event Log] ClearAggregate';
    constructor() { }
}