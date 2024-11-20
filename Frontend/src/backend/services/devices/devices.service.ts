import { Injectable } from "@angular/core";
import { DevicesApiRepositoryService } from "../../repositories/devices-api-repository.service";
import GetDevicesResponse from "./models/get-devices-response";
import GetDevicesRequest from "./models/get-devices-request";
import { Observable } from "rxjs";
import ImportDevicesResponse from "./models/import-devices-response";
import ImportDevicesRequest from "./models/import-devices-request";
import MoveDeviceResponse from "./models/move-device-response";
import MoveDeviceRequest from "./models/move-device-request";
import NameDeviceRequest from "./models/name-device-request";
import NameDeviceResponse from "./models/name-device-response";

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

    public moveDeviceToRoom(
        hardwareId: string,
        request: MoveDeviceRequest
    ): Observable<MoveDeviceResponse> {
        return this._repository.moveDeviceToRoom(hardwareId, request);
    }

    public nameDevice(
        hardwareId: string,
        request: NameDeviceRequest
    ): Observable<NameDeviceResponse> {
        return this._repository.nameDevice(hardwareId, request);
    }
}
