import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import CreateBusinessRequest from "../services/businesses/models/create-business-request";
import CreateBusinessResponse from "../services/businesses/models/create-business-response";

@Injectable({
    providedIn: "root"
})
export class BusinessesRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "businesses", http);
    }

    public postBusiness(request: CreateBusinessRequest): Observable<CreateBusinessResponse> {
        return this.post<CreateBusinessResponse>(request);
    }
}
