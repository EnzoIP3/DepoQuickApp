import { Injectable } from "@angular/core";
import { DeviceTypesApiRepositoryService } from "../../repositories/device-types-api-repository.service";
import { Observable } from "rxjs";
import DeviceTypesResponse from "./models/device-types-response";

@Injectable({
    providedIn: "root"
})
export class DeviceTypesService {
    constructor(
        private readonly _repository: DeviceTypesApiRepositoryService
    ) {}

    public getDeviceTypes(): Observable<DeviceTypesResponse> {
        return this._repository.getDeviceTypes();
    }
}
