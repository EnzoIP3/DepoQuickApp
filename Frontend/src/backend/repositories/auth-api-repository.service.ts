import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import AuthRequest from "../services/auth/models/auth-request";
import AuthResponse from "../services/auth/models/auth-response";

@Injectable({
    providedIn: "root"
})
export class AuthApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "auth", http);
    }

    public login(authRequest: AuthRequest): Observable<AuthResponse> {
        return this.post<AuthResponse>(authRequest);
    }
}
