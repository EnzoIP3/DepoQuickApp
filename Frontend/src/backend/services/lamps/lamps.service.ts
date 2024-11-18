import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LampsApiRepositoryService } from "../../repositories/lamps-api-repository.service";
import { CreateDeviceResponse } from "../devices/models/create-device-response";
import { CreateLampRequest } from "./models/create-lamp-request";

@Injectable({
    providedIn: "root"
})
export class LampsService {
    constructor(private readonly _repository: LampsApiRepositoryService) {}

    public postDevice(
        request: CreateLampRequest
    ): Observable<CreateDeviceResponse> {
        return this._repository.postDevice(request);
    }
}
