import { Component, OnDestroy, OnInit } from "@angular/core";
import { NotificationsService } from "../../backend/services/notifications/notifications.service";
import { DeviceTypesService } from "../../backend/services/device-types/device-types.service"; // Import the service
import { MessageService } from "primeng/api";
import NotificationData from "../../backend/services/notifications/models/notification-data";
import { Subscription } from "rxjs";
import GetNotificationsRequest from "../../backend/services/notifications/models/get-notifications-request";
import { CommonModule } from "@angular/common";
import { DropdownComponent } from "../../components/dropdown/dropdown.component";
import { MessageComponent } from "../../components/message/message.component";
import { SkeletonComponent } from "../../components/skeleton/skeleton.component";

@Component({
    selector: "app-notification-messages",
    standalone: true,
    templateUrl: "./notification-messages.component.html",
    imports: [
        CommonModule,
        DropdownComponent,
        MessageComponent,
        SkeletonComponent
    ]
})
export class NotificationMessagesComponent implements OnInit, OnDestroy {
    constructor(
        private readonly _notificationsService: NotificationsService,
        private readonly _messageService: MessageService,
        private readonly _deviceTypesService: DeviceTypesService
    ) {}

    private _notificationsSubscription: Subscription | null = null;

    notifications: NotificationData[] = [];
    loading = true;
    loadingArray = Array(5);
    filters: Partial<GetNotificationsRequest> = { read: false };
    devices: { label: string; value: string }[] = [];
    dates: { label: string; value: string }[] = [];
    readStates = [
        { label: "Read", value: true },
        { label: "Unread", value: false }
    ];

    ngOnInit() {
        this.fetchNotifications();
        this.fetchDeviceTypes();
    }

    fetchNotifications() {
        this.loading = true;
        const queryParams = this.buildQueryParams();
        this.unsubscribeNotifications();
        this._notificationsSubscription = this._notificationsService
            .getNotifications(queryParams)
            .subscribe({
                next: (response) => {
                    this.loading = false;
                    this.notifications = response.notifications;
                    if (this.dates.length === 0) {
                        this.initializeFilterOptions(response.notifications);
                    }
                },
                error: (error) => this.handleError(error)
            });
    }

    fetchDeviceTypes() {
        this._deviceTypesService.getDeviceTypes().subscribe({
            next: (response) => {
                this.devices = response.deviceTypes.map((type) => ({
                    label: type,
                    value: type
                }));
            },
            error: (error) => {
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: `Failed to load device types: ${error.message}`
                });
            }
        });
    }

    initializeFilterOptions(notifications: NotificationData[]) {
        this.dates = this.getUniqueOptions(
            notifications,
            (n) => this.formatDate(n.dateCreated),
            (n) => this.formatDate(n.dateCreated)
        );
    }

    private getUniqueOptions<T>(
        items: T[],
        valueSelector: (item: T) => string,
        labelSelector: (item: T) => string
    ): { label: string; value: string }[] {
        const uniqueMap = new Map<string, string>();
        items.forEach((item) => {
            const value = valueSelector(item);
            if (!uniqueMap.has(value)) {
                uniqueMap.set(value, labelSelector(item));
            }
        });
        return Array.from(uniqueMap, ([value, label]) => ({ label, value }));
    }

    buildQueryParams(): Partial<GetNotificationsRequest> {
        const queryParams: Partial<GetNotificationsRequest> = {};

        if (this.filters.device) {
            queryParams.device = this.filters.device;
        }
        if (this.filters.dateCreated) {
            queryParams.dateCreated = this.filters.dateCreated;
        }
        if (typeof this.filters.read === "boolean") {
            queryParams.read = this.filters.read;
        }

        return queryParams;
    }

    formatDate(dateString: string): string {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, "0");
        const month = String(date.getMonth() + 1).padStart(2, "0");
        const year = date.getFullYear();
        return `${day}-${month}-${year}`;
    }

    handleError(error: any) {
        this.loading = false;
        this._messageService.add({
            severity: "error",
            summary: "Error",
            detail: error.message
        });
    }

    onDeviceFilterChange(device: string) {
        this.filters.device = device;
        this.fetchNotifications();
    }

    onDateFilterChange(dateCreated: string) {
        this.filters.dateCreated = dateCreated;
        this.fetchNotifications();
    }

    onReadFilterChange(read: boolean | null) {
        this.filters.read = read === null ? undefined : read;
        this.fetchNotifications();
    }

    unsubscribeNotifications() {
        this._notificationsSubscription?.unsubscribe();
    }

    getNotificationDetail(notification: NotificationData): string {
        return `Generated by ${notification.device.name} at ${notification.dateCreated}`;
    }

    ngOnDestroy() {
        this.unsubscribeNotifications();
    }
}
