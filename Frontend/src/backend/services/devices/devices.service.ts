import { Injectable } from "@angular/core";
import { DevicesApiRepositoryService } from "../../repositories/devices-api-repository.service";
import GetDevicesResponse from "./models/get-devices-response";
import GetDevicesRequest from "./models/get-devices-request";
import ImportDevicesResponse from "./models/import-devices-response";
import { Observable } from "rxjs";
import ImportDevicesRequest from "./models/import-devices-request";

@Injectable({
    providedIn: "root"
})
export class DevicesService {
    constructor(private readonly _repository: DevicesApiRepositoryService) {}

    public getDevices(
        request?: GetDevicesRequest
    ): Observable<GetDevicesResponse> {
        return this._repository.getDevices(request);
    }

    public importDevices(
        request: ImportDevicesRequest
    ): Observable<ImportDevicesResponse> {
        return this._repository.importDevices(request);
    }
}
