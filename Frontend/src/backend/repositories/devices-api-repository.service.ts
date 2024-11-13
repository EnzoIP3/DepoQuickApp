import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import DevicesResponse from "../services/devices/models/devices-response";
import { Observable } from "rxjs";
import DevicesRequest from "../services/devices/models/devices-request";

@Injectable({
    providedIn: "root"
})
export class DevicesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "devices", http);
    }

    public getDevices(request?: DevicesRequest): Observable<DevicesResponse> {
        return this.get<DevicesResponse>({ queries: request });
    }
}
