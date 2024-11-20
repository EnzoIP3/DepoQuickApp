import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { DeviceImporterFilesApiRepositoryService } from "../../repositories/device-importer-files-api-repository.service";
import GetImportFilesResponse from "./models/get-import-files-response";

@Injectable({
    providedIn: "root"
})
export class DeviceImportFilesService {
    constructor(
        private readonly _repository: DeviceImporterFilesApiRepositoryService
    ) {}

    public getImportFiles(): Observable<GetImportFilesResponse> {
        return this._repository.getImportFiles();
    }
}
