import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import GetValidatorsResponse from "./models/get-validators-response";
import { DeviceValidatorsApiRepositoryService } from "../../repositories/device-validators-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class DeviceValidatorsService {
    constructor(private readonly _repository: DeviceValidatorsApiRepositoryService) {}

    public getDeviceValidators(): Observable<GetValidatorsResponse> {
        return this._repository.getDeviceValidators();
    }
}
