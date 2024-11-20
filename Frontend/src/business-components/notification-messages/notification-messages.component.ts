import { Component } from "@angular/core";
import { NotificationsService } from "../../backend/services/notifications/notifications.service";
import { MessageService } from "primeng/api";
import NotificationData from "../../backend/services/notifications/models/notification-data";
import { Subscription } from "rxjs";
import { MessageComponent } from "../../components/message/message.component";
import { CommonModule } from "@angular/common";
import { SkeletonComponent } from "../../components/skeleton/skeleton.component";

@Component({
    selector: "app-notification-messages",
    standalone: true,
    imports: [MessageComponent, CommonModule, SkeletonComponent],
    templateUrl: "./notification-messages.component.html"
})
export class NotificationMessagesComponent {
    constructor(
        private readonly _notificationsService: NotificationsService,
        private readonly _messageService: MessageService
    ) {}

    private _notificationsSubscription: Subscription | null = null;
    notifications: NotificationData[] = [];
    loading: boolean = true;
    loadingArray: any[] = new Array(5);

    ngOnInit() {
        this._notificationsSubscription = this._notificationsService
            .getNotifications()
            .subscribe({
                next: (response) => {
                    this.loading = false;
                    this.notifications = response.notifications;
                },
                error: (error) => {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._notificationsSubscription?.unsubscribe();
    }
}
