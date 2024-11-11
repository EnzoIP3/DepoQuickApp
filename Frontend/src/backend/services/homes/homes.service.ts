import { Injectable } from "@angular/core";
import { HomesApiRepositoryService } from "../../repositories/homes-api-repository.service";
import { Observable } from "rxjs";
import Home from "./models/home";

@Injectable({
    providedIn: "root"
})
export class HomesService {
    constructor(private readonly _repository: HomesApiRepositoryService) {}

    public getHomes(): Observable<Home[]> {
        return this._repository.getHomes();
    }
}
