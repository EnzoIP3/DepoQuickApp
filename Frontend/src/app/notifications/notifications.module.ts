import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NotificationsRoutingModule } from "./notifications-routing.module";
import { NotificationsPageComponent } from "./notifications-page/notifications-page.component";
import { NotificationMessagesComponent } from "../../business-components/notification-messages/notification-messages.component";
import { PanelComponent } from "../../components/panel/panel.component";

@NgModule({
    declarations: [NotificationsPageComponent],
    imports: [
        CommonModule,
        NotificationsRoutingModule,
        NotificationMessagesComponent,
        PanelComponent
    ]
})
export class NotificationsModule {}
