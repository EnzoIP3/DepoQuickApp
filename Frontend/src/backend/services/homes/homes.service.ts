import { Injectable } from "@angular/core";
import { HomesApiRepositoryService } from "../../repositories/homes-api-repository.service";
import { BehaviorSubject, Observable, tap } from "rxjs";
import GetHomesResponse from "./models/get-homes-response";
import GetHomeResponse from "./models/get-home-response";
import AddHomeRequest from "./models/add-home-request";
import AddHomeResponse from "./models/add-home-response";
import AddMemberRequest from "./models/add-member-request";
import AddMemberResponse from "./models/add-member-response";
import GetMembersResponse from "./models/get-members-response";

@Injectable({
    providedIn: "root"
})
export class HomesService {
    private _members$ = new BehaviorSubject<GetMembersResponse | null>(null);

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

    public addMember(
        homeId: string,
        addMemberRequest: AddMemberRequest
    ): Observable<AddMemberResponse> {
        return this._repository.addMember(homeId, addMemberRequest).pipe(
            tap(() => {
                this.getMembers(homeId).subscribe();
            })
        );
    }

    public getMembers(homeId: string): Observable<GetMembersResponse> {
        return this._repository.getMembers(homeId).pipe(
            tap((members) => {
                this._members$.next(members);
            })
        );
    }

    get members(): Observable<GetMembersResponse | null> {
        return this._members$.asObservable();
    }
}
