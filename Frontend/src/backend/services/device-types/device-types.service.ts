import { Injectable } from "@angular/core";
import { DeviceTypesApiRepositoryService } from "../../repositories/device-types-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class DeviceTypesService {
    constructor(
        private readonly _repository: DeviceTypesApiRepositoryService
    ) {}

    public getDeviceTypes() {
        return this._repository.getDeviceTypes();
    }
}
