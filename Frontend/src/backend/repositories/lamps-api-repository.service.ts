import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import { CreateDeviceResponse } from "../services/devices/models/create-device-response";
import { CreateLampRequest } from "../services/lamps/models/create-lamp-request";
import ApiRepository from "./api-repository";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class LampsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environment.apiUrl, "lamps", http, router);
    }

    public addDevice(
        request: CreateLampRequest
    ): Observable<CreateDeviceResponse> {
        return this.post<CreateDeviceResponse>(request);
    }
}
