import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import { Observable } from "rxjs";
import GetBusinessesRequest from "../services/businesses/models/business-request";
import GetBusinessesResponse from "../services/businesses/models/business-response";

@Injectable({
    providedIn: "root"
})
export class BusinessesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environments.apiUrl, "businesses", http);
    }

    public getBusinesses(
        request?: GetBusinessesRequest
    ): Observable<GetBusinessesResponse> {
        return this.get<GetBusinessesResponse>({ queries: request });
    }
}
