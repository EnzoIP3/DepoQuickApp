import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import GetUsersRequest from "../services/users/models/user-request";
import GetUsersResponse from "../services/users/models/user-response";
import ApiRepository from "./api-repository";
import GetBusinessRequest from "../services/users/models/get-business-request";
import { GetBusinessResponse } from "../services/users/models/get-business-response";
import { Router } from "@angular/router";
import HomeOwnerRoleResponse from "../services/users/models/home-owner-role-response";

@Injectable({
    providedIn: "root"
})
export class UsersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "users", http, router);
    }

    public getUsers(request?: GetUsersRequest): Observable<GetUsersResponse> {
        return this.get<GetUsersResponse>({ queries: request });
    }

    public getBusinesses(
        userId: string,
        request: GetBusinessRequest
    ): Observable<GetBusinessResponse> {
        return this.get<GetBusinessResponse>({
            extraResource: `${userId}/businesses`,
            queries: request
        });
    }

    public addHomeOwnerRole(): Observable<HomeOwnerRoleResponse> {
        return this.patch<HomeOwnerRoleResponse>(null, "home_owner_role");
    }
}
