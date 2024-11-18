import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import ApiRepository, { RequestConfig } from "./api-repository";
import GetBusinessRequest from "../services/users/models/get-business-request";
import { GetBusinessResponse } from "../services/users/models/get-business-response";

@Injectable({
    providedIn: "root"
})
export class UsersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "users", http);
    }

    public getBusiness(
        userId: string,
        request: GetBusinessRequest
    ): Observable<GetBusinessResponse> {
        return this.get<GetBusinessResponse>({
            extraResource: `${userId}/businesses`,
            queries: request
        });
    }
}
