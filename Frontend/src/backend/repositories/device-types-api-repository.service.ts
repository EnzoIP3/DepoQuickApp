import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import DeviceTypesResponse from "../services/device-types/models/device-types-response";

@Injectable({
    providedIn: "root"
})
export class DeviceTypesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "device-types", http);
    }

    public getDeviceTypes(): Observable<DeviceTypesResponse[]> {
        return this.get<DeviceTypesResponse[]>();
    }
}
