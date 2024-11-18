import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import AddDeviceToRoomRequest from "../services/rooms/models/add-device-to-room-request";
import { Observable } from "rxjs";
import AddDeviceToRoomResponse from "../services/rooms/models/add-device-to-room-response";

@Injectable({
    providedIn: "root"
})
export class RoomsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "rooms", http);
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
