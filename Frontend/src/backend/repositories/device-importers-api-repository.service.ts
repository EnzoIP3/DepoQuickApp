import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import { GetImportersResponse } from "../services/importers/models/get-importers-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class DeviceImportersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environment.apiUrl, "device_importers", http, router);
    }

    public getImporters(): Observable<GetImportersResponse> {
        return this.get<GetImportersResponse>();
    }
}
