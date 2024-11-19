import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BusinessesApiRepositoryService } from "../../repositories/businesses-api-repository.js";
import GetBusinessesRequest from "./models/business-request.js";
import GetBusinessesResponse from "./models/business-response.js";

@Injectable({
    providedIn: "root"
})
export class BusinessesService {
    constructor(private readonly _repository: BusinessesApiRepositoryService) {}

    public getUsers(request?: GetBusinessesRequest): Observable<GetBusinessesResponse> {
        return this._repository.getBusinesses(request);
    }
}
