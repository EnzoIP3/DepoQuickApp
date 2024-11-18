import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import CreateBusinessRequest from "./models/create-business-request";
import CreateBusinessResponse from "./models/create-business-response";
import { BusinessesApiRepositoryService } from "../../repositories/businesses-api-repository.service";
import UpdateValidatorResponse from "./models/update-validator-response";
import UpdateValidatorRequest from "./models/update-validator-request";
import GetDevicesResponse from "../devices/models/get-devices-response";
import GetBusinessDevicesRequest from "./models/get-business-devices-request";
@Injectable({
    providedIn: "root"
})
export class BusinessesService {
    constructor(private readonly _repository: BusinessesApiRepositoryService) {}

    public postBusiness(
        request: CreateBusinessRequest
    ): Observable<CreateBusinessResponse> {
        return this._repository.postBusiness(request);
    }

    public updateValidator(
        businessId: string,
        request: UpdateValidatorRequest
    ): Observable<UpdateValidatorResponse> {
        return this._repository.updateValidator(businessId, request);
    }

    public getDevices(
        businessId: string,
        request: GetBusinessDevicesRequest
    ): Observable<GetDevicesResponse> {
        return this._repository.getDevices(businessId, request);
    }
}
