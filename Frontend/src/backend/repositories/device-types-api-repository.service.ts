import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import { Observable } from "rxjs";
import DeviceTypesResponse from "../services/device-types/models/device-types-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class DeviceTypesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "device_types", http, router);
    }

    public getDeviceTypes(): Observable<DeviceTypesResponse> {
        return this.get<DeviceTypesResponse>();
    }
}
