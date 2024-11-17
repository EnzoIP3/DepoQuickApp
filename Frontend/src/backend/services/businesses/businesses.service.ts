import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import CreateBusinessRequest from "./models/create-business-request";
import CreateBusinessResponse from "./models/create-business-response";
import { BusinessesApiRepositoryService } from "../../repositories/businesses-api-repository.service";
import UpdateValidatorResponse from "./models/update-validator-response";
import UpdateValidatorRequest from "./models/update-validator-request";

@Injectable({
    providedIn: "root"
})
export class BusinessesService {
    constructor(private readonly _repository: BusinessesApiRepositoryService) {}

    public postBusiness(request: CreateBusinessRequest): Observable<CreateBusinessResponse> {
        return this._repository.postBusiness(request);
    }

    public updateValidator(businessId: string, request: UpdateValidatorRequest): Observable<UpdateValidatorResponse> {
        return this._repository.updateValidator(businessId, request);
    }
}
