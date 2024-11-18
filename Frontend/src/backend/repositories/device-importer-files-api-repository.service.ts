import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { Observable } from "rxjs";
import GetImportFilesResponse from "../services/importers/models/get-import-files-response";

@Injectable({
    providedIn: "root"
})
export class DeviceImporterFilesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "device_import_files", http);
    }

    public getImportFiles(): Observable<GetImportFilesResponse> {
        return this.get<GetImportFilesResponse>();
    }
}
