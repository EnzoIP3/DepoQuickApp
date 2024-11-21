import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import AddRequest from "../services/business-owners/models/add-request";
import AddResponse from "../services/business-owners/models/add-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class BusinessOwnersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environment.apiUrl, "business_owners", http, router);
    }

    public add(registerRequest: AddRequest): Observable<AddResponse> {
        return this.post<AddResponse>(registerRequest);
    }
}
