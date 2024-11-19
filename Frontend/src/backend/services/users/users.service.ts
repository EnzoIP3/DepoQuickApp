import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { UsersApiRepositoryService } from "../../repositories/users-api-repository.service";
import GetBusinessRequest from "./models/get-business-request";
import { GetBusinessResponse } from "./models/get-business-response";

@Injectable({
    providedIn: "root"
})
export class UsersService {
    constructor(private readonly _repository: UsersApiRepositoryService) {}

    public getBusiness(
        request: GetBusinessRequest,
        userId: string
    ): Observable<GetBusinessResponse> {
        return this._repository.getBusiness(userId, request);
    }
}
