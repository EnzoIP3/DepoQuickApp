import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { NotificationsRoutingModule } from "./notifications-routing.module";
import { NotificationsPageComponent } from "./notifications-page/notifications-page.component";

@NgModule({
    declarations: [NotificationsPageComponent],
    imports: [CommonModule, NotificationsRoutingModule]
})
export class NotificationsModule {}
