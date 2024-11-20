import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NotificationsRoutingModule } from "./notifications-routing.module";
import { NotificationsPageComponent } from "./notifications-page/notifications-page.component";
import { NotificationMessagesComponent } from "../../business-components/notification-messages/notification-messages.component";

@NgModule({
    declarations: [NotificationsPageComponent],
    imports: [
        CommonModule,
        NotificationsRoutingModule,
        NotificationMessagesComponent
    ]
})
export class NotificationsModule {}
