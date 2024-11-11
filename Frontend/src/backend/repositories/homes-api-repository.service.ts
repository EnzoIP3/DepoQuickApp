import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import DeviceTypesResponse from "../services/device-types/models/device-types-response";
import ApiRepository from "./api-repository";
import Home from "../services/homes/models/home";

@Injectable({
    providedIn: "root"
})
export class HomesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "homes", http);
    }

    public getHomes(): Observable<Home[]> {
        return this.get<Home[]>();
    }
}
