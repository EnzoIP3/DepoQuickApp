import { Injectable } from "@angular/core";
import { UsersApiRepositoryService } from "../../repositories/users-api-repository.service.js";
import { Observable } from "rxjs";
import GetUsersRequest from "./models/user-request.js";
import GetUsersResponse from "./models/user-response.js";

@Injectable({
    providedIn: "root"
})
export class UsersService {
    constructor(private readonly _repository: UsersApiRepositoryService) {}

    public getUsers(request?: GetUsersRequest): Observable<GetUsersResponse> {
        return this._repository.getUsers(request);
    }
}
