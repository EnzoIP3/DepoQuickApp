import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DeviceImportersApiRepositoryService } from "../../repositories/device-importers-api-repository.service";
import GetImportersResponse from "./models/get-importers-response";

@Injectable({
    providedIn: "root"
})
export class DeviceImportersService {
    constructor(
        private readonly _repository: DeviceImportersApiRepositoryService
    ) {}

    public getImporters(): Observable<GetImportersResponse> {
        return this._repository.getImporters();
    }
}
