import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import GetDevicesResponse from "../services/devices/models/get-devices-response";
import { Observable } from "rxjs";
import GetDevicesRequest from "../services/devices/models/get-devices-request";
import MoveDeviceRequest from "../services/devices/models/move-device-request";
import MoveDeviceResponse from "../services/devices/models/move-device-response";

@Injectable({
    providedIn: "root"
})
export class DevicesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "devices", http);
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
}
