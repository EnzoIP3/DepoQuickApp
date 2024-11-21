import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { UsersApiRepositoryService } from "../../repositories/users-api-repository.service";
import GetBusinessRequest from "./models/get-business-request";
import { GetBusinessResponse } from "./models/get-business-response";
import GetUsersRequest from "./models/user-request";
import GetUsersResponse from "./models/user-response";
import HomeOwnerRoleResponse from "./models/home-owner-role-response";

@Injectable({
    providedIn: "root"
})
export class UsersService {
    constructor(private readonly _repository: UsersApiRepositoryService) {}

    private _users$: BehaviorSubject<GetUsersResponse | null> =
        new BehaviorSubject<GetUsersResponse | null>(null);

    public getBusinesses(
        request: GetBusinessRequest,
        userId: string
    ): Observable<GetBusinessResponse> {
        return this._repository.getBusinesses(userId, request);
    }

    public getUsers(request?: GetUsersRequest): Observable<GetUsersResponse> {
        return this._repository
            .getUsers(request)
            .pipe(tap((response) => this._users$.next(response)));
    }

    public addHomeOwnerRole(): Observable<HomeOwnerRoleResponse> {
        return this._repository.addHomeOwnerRole();
    }

    get users(): Observable<GetUsersResponse | null> {
        return this._users$.asObservable();
    }
}
