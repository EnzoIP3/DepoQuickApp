import { Injectable } from "@angular/core";
import { DevicesApiRepositoryService } from "../../repositories/devices-api-repository.service";
import DevicesResponse from "./models/devices-response";
import DevicesRequest from "./models/devices-request";
import { Observable } from "rxjs";

@Injectable({
    providedIn: "root"
})
export class DevicesService {
    constructor(private readonly _repository: DevicesApiRepositoryService) {}

    public getDevices(request?: DevicesRequest): Observable<DevicesResponse> {
        return this._repository.getDevices(request);
    }
}
