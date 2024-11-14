import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { environment } from "../../environments/environment";
import { HttpClient } from "@angular/common/http";
import SetNotificationsRequest from "../services/members/models/set-notifications-request";
import { Observable } from "rxjs";
import SetNotificationsResponse from "../services/members/models/set-notifications-response";

@Injectable({
    providedIn: "root"
})
export class MembersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "members", http);
    }

    public setNotifications(
        memberId: string,
        request: SetNotificationsRequest
    ): Observable<SetNotificationsResponse> {
        return this.patch<SetNotificationsResponse>(
            request,
            `${memberId}/notifications`
        );
    }
}
