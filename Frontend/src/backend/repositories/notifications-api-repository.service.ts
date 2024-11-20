import { Injectable } from "@angular/core";
import ApiRepository from "./api-repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment";
import GetNotificationsRequest from "../services/notifications/models/get-notifications-request";
import { Observable } from "rxjs";
import GetNotificationsResponse from "../services/notifications/models/get-notifications-response";

@Injectable({
    providedIn: "root"
})
export class NotificationsApiRepositoryService extends ApiRepository {
    constructor(http: HttpClient) {
        super(environment.apiUrl, "notifications", http);
    }

    public getNotifications(
        request: GetNotificationsRequest
    ): Observable<GetNotificationsResponse> {
        return this.get<GetNotificationsResponse>({ queries: request });
    }
}
