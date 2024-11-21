import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import environments from "../../environments";
import GetNotificationsRequest from "../services/notifications/models/get-notifications-request";
import { Observable } from "rxjs";
import GetNotificationsResponse from "../services/notifications/models/get-notifications-response";
import { Router } from "@angular/router";

@Injectable({
    providedIn: "root"
})
export class NotificationsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient, router: Router) {
        super(environments.apiUrl, "notifications", http, router);
    }

    public getNotifications(
        request?: GetNotificationsRequest
    ): Observable<GetNotificationsResponse> {
        return this.get<GetNotificationsResponse>({ queries: request });
    }
}
