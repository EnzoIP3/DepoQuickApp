import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import { CreateDeviceResponse } from "../services/devices/models/create-device-response";
import { CreateSensorRequest } from "../services/sensors/models/create-sensor-request";
import ApiRepository from "./api-repository";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class SensorsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "sensors", http, router);
    }

    public addDevice(
        request: CreateSensorRequest
    ): Observable<CreateDeviceResponse> {
        return this.post<CreateDeviceResponse>(request);
    }
}
