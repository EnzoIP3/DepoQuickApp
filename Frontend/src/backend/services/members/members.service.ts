import { Injectable } from "@angular/core";
import SetNotificationsRequest from "./models/set-notifications-request";
import { Observable } from "rxjs";
import SetNotificationsResponse from "./models/set-notifications-response";
import { MembersApiRepositoryService } from "../../repositories/members-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class MembersService {
    constructor(private readonly _repository: MembersApiRepositoryService) {}

    public setNotifications(
        memberId: string,
        request: SetNotificationsRequest
    ): Observable<SetNotificationsResponse> {
        return this._repository.setNotifications(memberId, request);
    }
}
