import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../../environments/environment";
import ApiRepository from "./api-repository";
import GetHomesResponse from "../services/homes/models/get-homes-response";
import GetHomeResponse from "../services/homes/models/get-home-response";
import AddHomeRequest from "../services/homes/models/add-home-request";
import AddHomeResponse from "../services/homes/models/add-home-response";
import AddMemberRequest from "../services/homes/models/add-member-request";
import AddMemberResponse from "../services/homes/models/add-member.response";

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

    public getHome(id: string): Observable<GetHomeResponse> {
        return this.get<GetHomeResponse>({ extraResource: id });
    }

    public addHome(
        addHomeRequest: AddHomeRequest
    ): Observable<AddHomeResponse> {
        return this.post<AddHomeResponse>(addHomeRequest);
    }

    public addMember(
        homeId: string,
        addMemberRequest: AddMemberRequest
    ): Observable<AddMemberResponse> {
        return this.post<AddMemberResponse>(
            addMemberRequest,
            `${homeId}/members`
        );
    }
}
