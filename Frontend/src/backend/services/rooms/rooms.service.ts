import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable } from "rxjs";
import AddRoomRequest from "../homes/models/add-room-request";
import AddRoomResponse from "../homes/models/add-room-response";
import { RoomsApiRepositoryService } from "../../repositories/rooms-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class RoomsService {
    constructor(private readonly _repository: RoomsApiRepositoryService) {}
}
