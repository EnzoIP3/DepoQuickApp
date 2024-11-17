import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import CreateBusinessRequest from "../services/businesses/models/create-business-request";
import CreateBusinessResponse from "../services/businesses/models/create-business-response";
import UpdateValidatorResponse from "../services/businesses/models/update-validator-request";
import UpdateValidatorRequest from "../services/businesses/models/update-validator-response";

@Injectable({
    providedIn: "root"
})
export class BusinessesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "businesses", http);
    }

    public postBusiness(request: CreateBusinessRequest): Observable<CreateBusinessResponse> {
        return this.post<CreateBusinessResponse>(request);
    }

    updateValidator(businessId: string, request: UpdateValidatorRequest): Observable<UpdateValidatorResponse> {
        return this.patch<UpdateValidatorResponse>(request, `${businessId}/validator`);
    }
}
