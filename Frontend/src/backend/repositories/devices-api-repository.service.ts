import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import GetDevicesResponse from "../services/devices/models/get-devices-response";
import { Observable } from "rxjs";
import GetDevicesRequest from "../services/devices/models/get-devices-request";
import ImportDevicesRequest from "../services/devices/models/import-devices-request";
import ImportDevicesResponse from "../services/devices/models/import-devices-response";

@Injectable({
    providedIn: "root"
})
export class DevicesApiRepositoryService extends ApiRepository {
    
    constructor(http: HttpClient) {
        super(environment.apiUrl, "devices", http);
    }

    public getDevices(request?: GetDevicesRequest): Observable<GetDevicesResponse> {
        return this.get<GetDevicesResponse>({ queries: request });
    }

    importDevices(request: ImportDevicesRequest): Observable<ImportDevicesResponse> {
        return this.post<ImportDevicesResponse>(request);
    }
}
