import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { catchError, Observable } from "rxjs";
import AddRequest from "../services/admins/models/add-request";
import AddResponse from "../services/admins/models/add-response";

@Injectable({
    providedIn: "root"
})
export class AdminsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "admins", http);
    }

    public add(
        registerRequest: AddRequest
    ): Observable<AddResponse> {
        return this.post<AddResponse>(registerRequest);
    }

    public delete(adminId: string): Observable<void> {
        const url = `${this.fullEndpoint}/${adminId}`;
        return this.http.delete<void>(url, this.headers).pipe(
            catchError(this.handleError)
        );
    }
    
}
