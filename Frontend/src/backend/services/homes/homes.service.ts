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
import AddDevicesRequest from "./models/add-devices-request";
import AddDevicesResponse from "./models/add-devices-response";
import GetHomeDevicesResponse from "./models/get-home-devices-response";
import NameHomeResponse from "./models/name-home-response";
import NameHomeRequest from "./models/name-home-request";
import AddRoomRequest from "./models/add-room-request";
import AddRoomResponse from "./models/add-room-response";
import GetRoomsResponse from "./models/get-rooms-response";

@Injectable({
    providedIn: "root"
})
export class HomesService {
    private _members$ = new BehaviorSubject<GetMembersResponse | null>(null);
    private _devices$ = new BehaviorSubject<GetHomeDevicesResponse | null>(
        null
    );
    private _home$ = new BehaviorSubject<GetHomeResponse | null>(null);
    private _rooms$ = new BehaviorSubject<GetRoomsResponse | null>(null);

    constructor(private readonly _repository: HomesApiRepositoryService) {}

    public getHomes(): Observable<GetHomesResponse> {
        return this._repository.getHomes();
    }

    public getHome(id: string): Observable<GetHomeResponse> {
        return this._repository.getHome(id).pipe(
            tap((home) => {
                this._home$.next(home);
            })
        );
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

    public addDevicesToHome(
        homeId: string,
        addDevicesRequest: AddDevicesRequest
    ): Observable<AddDevicesResponse> {
        return this._repository
            .addDevicesToHome(homeId, addDevicesRequest)
            .pipe(
                tap(() => {
                    this.getDevices(homeId).subscribe();
                })
            );
    }

    public getDevices(homeId: string): Observable<GetHomeDevicesResponse> {
        return this._repository.getDevices(homeId).pipe(
            tap((devices) => {
                this._devices$.next(devices);
            })
        );
    }

    public nameHome(
        homeId: string,
        nameHomeRequest: NameHomeRequest
    ): Observable<NameHomeResponse> {
        return this._repository.nameHome(homeId, nameHomeRequest).pipe(
            tap(() => {
                this.getHome(homeId).subscribe();
            })
        );
    }

    public addRoom(
        homeId: string,
        addRoomRequest: AddRoomRequest
    ): Observable<AddRoomResponse> {
        return this._repository.addRoom(homeId, addRoomRequest).pipe(
            tap(() => {
                this.getRooms(homeId).subscribe();
            })
        );
    }

    public getRooms(homeId: string): Observable<GetRoomsResponse> {
        return this._repository.getRooms(homeId).pipe(
            tap((rooms) => {
                this._rooms$.next(rooms);
            })
        );
    }

    get members(): Observable<GetMembersResponse | null> {
        return this._members$.asObservable();
    }

    get devices(): Observable<GetHomeDevicesResponse | null> {
        return this._devices$.asObservable();
    }

    get home(): Observable<GetHomeResponse | null> {
        return this._home$.asObservable();
    }

    get rooms(): Observable<GetRoomsResponse | null> {
        return this._rooms$.asObservable();
    }
}
