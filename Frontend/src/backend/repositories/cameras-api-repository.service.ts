import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import { CreateCameraRequest } from "../services/cameras/models/create-camera-request";
import { CreateDeviceResponse } from "../services/devices/models/create-device-response";
import ApiRepository from "./api-repository";
import { GetCameraResponse } from "../services/cameras/models/get-camera-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class CamerasApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "cameras", http, router);
    }

    public addDevice(
        request: CreateCameraRequest
    ): Observable<CreateDeviceResponse> {
        return this.post<CreateDeviceResponse>(request);
    }

    public getCameraDetails(deviceId: string): Observable<GetCameraResponse> {
        return this.get<GetCameraResponse>({ extraResource: `${deviceId}` });
    }
}
