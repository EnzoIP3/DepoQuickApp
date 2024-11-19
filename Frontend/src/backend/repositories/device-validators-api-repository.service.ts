import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import GetValidatorsResponse from "../services/device-validators/models/get-validators-response";

@Injectable({
    providedIn: "root"
})
export class DeviceValidatorsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "device_validators", http);
    }

    public getDeviceValidators(): Observable<GetValidatorsResponse> {
        return this.get<GetValidatorsResponse>();
    }
}
