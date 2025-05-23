import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import ApiRepository from "./api-repository";
import environments from "../../environments";
import { Observable } from "rxjs";
import GetImportFilesResponse from "../services/importers/models/get-import-files-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class DeviceImporterFilesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "device_import_files", http, router);
    }

    public getImportFiles(): Observable<GetImportFilesResponse> {
        return this.get<GetImportFilesResponse>();
    }
}
