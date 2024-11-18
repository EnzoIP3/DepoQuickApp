import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import GetUsersRequest from "../services/user/models/user-request";
import GetUsersResponse from "../services/user/models/user-response";

@Injectable({
    providedIn: "root"
})
export class UsersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "users", http);
    }

    public getUsers(request?: GetUsersRequest): Observable<GetUsersResponse> {
        return this.get<GetUsersResponse>({ queries: request });
    }
}
