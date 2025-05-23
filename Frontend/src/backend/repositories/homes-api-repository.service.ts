import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import environments from "../../environments";
import ApiRepository from "./api-repository";
import GetHomesResponse from "../services/homes/models/get-homes-response";
import GetHomeResponse from "../services/homes/models/get-home-response";
import AddHomeRequest from "../services/homes/models/add-home-request";
import AddHomeResponse from "../services/homes/models/add-home-response";
import AddMemberRequest from "../services/homes/models/add-member-request";
import AddMemberResponse from "../services/homes/models/add-member-response";
import GetMembersResponse from "../services/homes/models/get-members-response";
import AddDevicesRequest from "../services/homes/models/add-devices-request";
import AddDevicesResponse from "../services/homes/models/add-devices-response";
import GetHomeDevicesResponse from "../services/homes/models/get-home-devices-response";
import NameHomeRequest from "../services/homes/models/name-home-request";
import NameHomeResponse from "../services/homes/models/name-home-response";
import AddRoomRequest from "../services/homes/models/add-room-request";
import AddRoomResponse from "../services/homes/models/add-room-response";
import GetRoomsResponse from "../services/homes/models/get-rooms-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class HomesApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "homes", http, router);
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

    public getMembers(homeId: string): Observable<GetMembersResponse> {
        return this.get<GetMembersResponse>({
            extraResource: `${homeId}/members`
        });
    }

    public addDevicesToHome(
        homeId: string,
        addDevicesRequest: AddDevicesRequest
    ): Observable<AddDevicesResponse> {
        return this.post<AddDevicesResponse>(
            addDevicesRequest,
            `${homeId}/devices`
        );
    }

    public getDevices(homeId: string): Observable<GetHomeDevicesResponse> {
        return this.get<GetHomeDevicesResponse>({
            extraResource: `${homeId}/devices`
        });
    }

    public nameHome(
        homeId: string,
        nameHomeRequest: NameHomeRequest
    ): Observable<NameHomeResponse> {
        return this.patch<NameHomeResponse>(nameHomeRequest, `${homeId}/name`);
    }

    public addRoom(
        homeId: string,
        addRoomRequest: AddRoomRequest
    ): Observable<AddRoomResponse> {
        return this.post<AddRoomResponse>(addRoomRequest, `${homeId}/rooms`);
    }

    public getRooms(homeId: string): Observable<GetRoomsResponse> {
        return this.get<GetRoomsResponse>({
            extraResource: `${homeId}/rooms`
        });
    }
}
