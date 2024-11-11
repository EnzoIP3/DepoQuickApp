import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import ApiRepository from "./api-repository";
import GetHomesResponse from "../services/homes/models/get-homes-response";

@Injectable({
    providedIn: "root"
})
export class HomesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "homes", http);
    }

    public getHomes(): Observable<GetHomesResponse> {
        return this.get<GetHomesResponse>();
    }
}
