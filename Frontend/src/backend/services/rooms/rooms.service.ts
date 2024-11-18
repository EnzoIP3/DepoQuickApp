import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import AddRoomRequest from "../homes/models/add-room-request";
import AddRoomResponse from "../homes/models/add-room-response";
import { RoomsApiRepositoryService } from "../../repositories/rooms-api-repository.service";
import AddDeviceToRoomRequest from "./models/add-device-to-room-request";
import AddDeviceToRoomResponse from "./models/add-device-to-room-response";

@Injectable({
    providedIn: "root"
})
export class RoomsService {
    constructor(private readonly _repository: RoomsApiRepositoryService) {}

    public addDeviceToRoom(
        roomId: string,
        addDeviceToRoomRequest: AddDeviceToRoomRequest
    ): Observable<AddDeviceToRoomResponse> {
        return this._repository.addDeviceToRoom(roomId, addDeviceToRoomRequest);
    }
}
