import { Injectable } from "@angular/core";
import GetNotificationsRequest from "./models/get-notifications-request";
import GetNotificationsResponse from "./models/get-notifications-response";
import { Observable } from "rxjs";
import { NotificationsApiRepositoryService } from "../../repositories/notifications-api-repository.service";

@Injectable({
    providedIn: "root"
})
export class NotificationsService {
    constructor(
        private readonly _repository: NotificationsApiRepositoryService
    ) {}

    public getNotifications(
        request?: GetNotificationsRequest
    ): Observable<GetNotificationsResponse> {
        return this._repository.getNotifications(request);
    }
}
