import { Injectable } from "@angular/core";
import AddRequest from "./models/add-request";
import AddResponse from "./models/add-response";
import { Observable } from "rxjs";
import { AdminsApiRepositoryService } from "../../repositories/admins-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class AdminsService {
    constructor(private readonly _repository: AdminsApiRepositoryService) {}

    public addAdmin(addRequest: AddRequest): Observable<AddResponse> {
        return this._repository.add(addRequest);
    }

    public deleteAdmin(id: string): Observable<void> {
        return this._repository.delete(id);
    }
}
