import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import AddDeviceToRoomRequest from "../services/rooms/models/add-device-to-room-request";
import { Observable } from "rxjs";
import AddDeviceToRoomResponse from "../services/rooms/models/add-device-to-room-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class RoomsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "rooms", http, router);
    }

    public addDeviceToRoom(
        roomId: string,
        addDeviceToRoomRequest: AddDeviceToRoomRequest
    ): Observable<AddDeviceToRoomResponse> {
        return this.post<AddDeviceToRoomResponse>(
            addDeviceToRoomRequest,
            `${roomId}/devices`
        );
    }
}
