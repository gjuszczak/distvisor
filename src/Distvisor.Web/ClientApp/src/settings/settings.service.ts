import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { ApplicationPaths } from "./settings.constants";
import { map } from "rxjs/operators";

@Injectable({
    providedIn: 'root'
})
export class SettingsService {

    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    }

    public getUpdates(): Observable<string[]> {
        return this.http.get(this.baseUrl + ApplicationPaths.ApiGetUpdates)
            .pipe(map(x => <string[]>x));
    }

    public update(tag: string): Observable<string> {
        return this.http.post(this.baseUrl + ApplicationPaths.ApiUpdate, null, { params: { tag }, responseType: "text" })
            .pipe(map(x => <string>x));
    }
}  