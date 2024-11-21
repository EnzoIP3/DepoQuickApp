import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import GetDevicesResponse from "../services/devices/models/get-devices-response";
import { Observable } from "rxjs";
import GetDevicesRequest from "../services/devices/models/get-devices-request";
import ImportDevicesRequest from "../services/devices/models/import-devices-request";
import ImportDevicesResponse from "../services/devices/models/import-devices-response";
import MoveDeviceRequest from "../services/devices/models/move-device-request";
import MoveDeviceResponse from "../services/devices/models/move-device-response";
import NameDeviceRequest from "../services/devices/models/name-device-request";
import NameDeviceResponse from "../services/devices/models/name-device-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class DevicesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "devices", http, router);
    }

    public getDevices(
        request?: GetDevicesRequest
    ): Observable<GetDevicesResponse> {
        return this.get<GetDevicesResponse>({ queries: request });
    }

    public moveDeviceToRoom(
        hardwareId: string,
        request: MoveDeviceRequest
    ): Observable<MoveDeviceResponse> {
        return this.patch<MoveDeviceResponse>(request, `${hardwareId}/room`);
    }

    public importDevices(
        request: ImportDevicesRequest
    ): Observable<ImportDevicesResponse> {
        return this.post<ImportDevicesResponse>(request);
    }

    public nameDevice(
        hardwareId: string,
        request: NameDeviceRequest
    ): Observable<
        import("../services/devices/models/name-device-response").default
    > {
        return this.patch<NameDeviceResponse>(request, `${hardwareId}/name`);
    }
}
