import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import { CreateDeviceResponse } from "../services/devices/models/create-device-response";
import ApiRepository from "./api-repository";
import { CreateMotionSensorRequest } from "../services/movement-sensors/models/create-motion-sensor-request";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class MotionSensorsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "motion_sensors", http, router);
    }

    public addDevice(
        request: CreateMotionSensorRequest
    ): Observable<CreateDeviceResponse> {
        return this.post<CreateDeviceResponse>(request);
    }
}
