import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import environments from "../../environments";
import { HttpClient } from "@angular/common/http";
import SetNotificationsRequest from "../services/members/models/set-notifications-request";
import { Observable } from "rxjs";
import SetNotificationsResponse from "../services/members/models/set-notifications-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class MembersApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "members", http, router);
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
