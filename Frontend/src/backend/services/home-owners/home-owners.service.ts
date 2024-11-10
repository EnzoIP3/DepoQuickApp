import { Injectable } from "@angular/core";
import { HomeOwnersApiRepositoryService } from "../../repositories/home-owners-api-repository.service";
import RegisterRequest from "./models/register-request";
import RegisterResponse from "./models/register-response";
import { Observable } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class HomeOwnersService {
    constructor(private readonly _repository: HomeOwnersApiRepositoryService) {}

    public register(
        registerRequest: RegisterRequest
    ): Observable<RegisterResponse> {
        return this._repository.register(registerRequest);
    }
}
