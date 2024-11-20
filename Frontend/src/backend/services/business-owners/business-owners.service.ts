import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BusinessOwnersApiRepositoryService } from "../../repositories/business-owner-repository.service";
import AddRequest from "./models/add-request";
import AddResponse from "./models/add-response";

@Injectable({
    providedIn: "root"
})
export class BusinessOwnersService {
    constructor(private readonly _repository: BusinessOwnersApiRepositoryService) {}

    public addBusinessOwner(
        addRequest: AddRequest
    ): Observable<AddResponse> {
        return this._repository.add(addRequest);
    }
}