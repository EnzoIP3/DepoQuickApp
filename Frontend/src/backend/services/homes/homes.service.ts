import { Injectable } from "@angular/core";
import { HomesApiRepositoryService } from "../../repositories/homes-api-repository.service";
import { Observable } from "rxjs";
import GetHomesResponse from "./models/get-homes-response";

@Injectable({
    providedIn: "root"
})
export class HomesService {
    constructor(private readonly _repository: HomesApiRepositoryService) {}

    public getHomes(): Observable<GetHomesResponse> {
        return this._repository.getHomes();
    }
}
