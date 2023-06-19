import { Injectable } from "@angular/core";
import { Action, Selector, State, StateContext } from "@ngxs/store";
import { of } from "rxjs";
import { catchError, map } from 'rxjs/operators';

import { AggregateDto } from "src/app/api/models";
import { EventLogService } from "src/app/api/services";
import { isGuid } from "src/app/shared";

import { ClearAggregate, LoadAggregate, LoadAggregateFail, LoadAggregateSuccess } from "./aggregate.actions";

export interface AggregateStateModel extends AggregateDto {
    loading: boolean,
    error: string,
};

export const aggregateStateDefaults: AggregateStateModel = {
    loading: false,
    error: '',
};

@State<AggregateStateModel>({
    name: 'aggregate',
    defaults: aggregateStateDefaults,
})
@Injectable()
export class AggregateState {
    constructor(private readonly eventLogService: EventLogService) { }

    @Selector()
    static getAggregate(state: AggregateStateModel) {
        return state;
    }

    @Action(LoadAggregate)
    loadAggregate({ dispatch, patchState }: StateContext<AggregateStateModel>, action: LoadAggregate) {
        if (isGuid(action.aggregateId)) {
            patchState({
                loading: true,
                error: '',
            });

            return this.eventLogService.apiEventLogAggregatesAggregateIdGet$Json({ aggregateId: action.aggregateId }).pipe(
                map(aggregate => dispatch(new LoadAggregateSuccess(aggregate))),
                catchError(error => {
                    dispatch(new LoadAggregateFail(error));
                    return of(new LoadAggregateFail(error));
                })
            );
        }

        return of(new LoadAggregateFail({ message: "Invalid aggregateId" }));
    }

    @Action(LoadAggregateSuccess)
    loadAggregateSuccess({ patchState }: StateContext<AggregateStateModel>, { aggregate }: LoadAggregateSuccess) {
        patchState({
            aggregateId: aggregate?.aggregateId ?? undefined,
            version: aggregate?.version ?? undefined,
            aggregateType: aggregate?.aggregateType ?? undefined,
            aggregateTypeDisplayName: aggregate?.aggregateTypeDisplayName ?? undefined,
            maskedPayload: aggregate?.maskedPayload ?? undefined,
            loading: false,
            error: ''
        });
    }

    @Action(LoadAggregateFail)
    loadAggregateFail({ patchState }: StateContext<AggregateStateModel>, { error }: LoadAggregateFail) {
        patchState({
            loading: false,
            error: error.message
        });
    }

    @Action(ClearAggregate)
    clearAggregate({ setState }: StateContext<AggregateStateModel>) {
        setState(aggregateStateDefaults);
    }
}