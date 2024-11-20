import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import RegisterRequest from "../services/home-owners/models/register-request";
import RegisterResponse from "../services/home-owners/models/register-response";
import { Observable } from "rxjs";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class HomeOwnersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environment.apiUrl, "home_owners", http, router);
    }

    public register(
        registerRequest: RegisterRequest
    ): Observable<RegisterResponse> {
        return this.post<RegisterResponse>(registerRequest);
    }
}
