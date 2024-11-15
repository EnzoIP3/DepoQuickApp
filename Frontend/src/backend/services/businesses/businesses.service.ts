import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import CreateBusinessRequest from "./models/create-business-request";
import CreateBusinessResponse from "./models/create-business-response";
import { BusinessesRepositoryService } from "../../repositories/businesses-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class BusinessesService {
    constructor(private readonly _repository: BusinessesRepositoryService) {}

    public postBusiness(request: CreateBusinessRequest): Observable<CreateBusinessResponse> {
        return this._repository.postBusiness(request);
    }
}
