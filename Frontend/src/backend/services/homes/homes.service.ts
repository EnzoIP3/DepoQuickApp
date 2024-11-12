import { Injectable } from "@angular/core";
import { HomesApiRepositoryService } from "../../repositories/homes-api-repository.service";
import { Observable } from "rxjs";
import GetHomesResponse from "./models/get-homes-response";
import GetHomeResponse from "./models/get-home-response";
import AddHomeRequest from "./models/add-home-request";
import AddHomeResponse from "./models/add-home-response";

@Injectable({
    providedIn: "root"
})
export class HomesService {
    constructor(private readonly _repository: HomesApiRepositoryService) {}

    public getHomes(): Observable<GetHomesResponse> {
        return this._repository.getHomes();
    }

    public getHome(id: string): Observable<GetHomeResponse> {
        return this._repository.getHome(id);
    }

    public addHome(
        addHomeRequest: AddHomeRequest
    ): Observable<AddHomeResponse> {
        return this._repository.addHome(addHomeRequest);
    }
}
