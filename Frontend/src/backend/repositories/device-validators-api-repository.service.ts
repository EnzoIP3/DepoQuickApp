import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import { Observable } from "rxjs";
import GetValidatorsResponse from "../services/device-validators/models/get-validators-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class DeviceValidatorsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "device_validators", http, router);
    }

    public getDeviceValidators(): Observable<GetValidatorsResponse> {
        return this.get<GetValidatorsResponse>();
    }
}
