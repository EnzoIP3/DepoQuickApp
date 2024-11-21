import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import { Observable } from "rxjs";
import AuthRequest from "../services/auth/models/auth-request";
import AuthResponse from "../services/auth/models/auth-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class AuthApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "auth", http, router);
    }

    public login(authRequest: AuthRequest): Observable<AuthResponse> {
        return this.post<AuthResponse>(authRequest);
    }
}
