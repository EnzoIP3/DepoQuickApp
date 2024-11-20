import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import CreateBusinessRequest from "../services/businesses/models/create-business-request";
import CreateBusinessResponse from "../services/businesses/models/create-business-response";
import UpdateValidatorResponse from "../services/businesses/models/update-validator-request";
import UpdateValidatorRequest from "../services/businesses/models/update-validator-response";
import GetBusinessDevicesRequest from "../services/businesses/models/get-business-devices-request";
import GetDevicesResponse from "../services/devices/models/get-devices-response";
import { Router } from "@angular/router";
import GetBusinessesRequest from "../services/businesses/models/business-request";
import GetBusinessesResponse from "../services/businesses/models/business-response";

@Injectable({
    providedIn: "root"
})
export class BusinessesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environment.apiUrl, "businesses", http, router);
    }

    public getBusinesses(
        request?: GetBusinessesRequest
    ): Observable<GetBusinessesResponse> {
        return this.get<GetBusinessesResponse>({ queries: request });
    }

    public postBusiness(
        request: CreateBusinessRequest
    ): Observable<CreateBusinessResponse> {
        return this.post<CreateBusinessResponse>(request);
    }

    public updateValidator(
        businessId: string,
        request: UpdateValidatorRequest
    ): Observable<UpdateValidatorResponse> {
        return this.patch<UpdateValidatorResponse>(
            request,
            `${businessId}/validator`
        );
    }

    public getDevices(
        businessId: string,
        request: GetBusinessDevicesRequest
    ): Observable<GetDevicesResponse> {
        return this.get<GetDevicesResponse>({
            queries: request,
            extraResource: `${businessId}/devices`
        });
    }
}
