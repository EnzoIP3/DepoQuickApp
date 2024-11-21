import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ButtonComponent } from "../../components/button/button.component";
import Member from "../../backend/services/homes/models/member";
import { MembersService } from "../../backend/services/members/members.service";
import SetNotificationsResponse from "../../backend/services/members/models/set-notifications-response";
import { MessagesService } from "../../backend/services/messages/messages.service";

@Component({
    selector: "app-set-notifications-button",
    standalone: true,
    imports: [CommonModule, ButtonComponent],
    templateUrl: "./set-notifications-button.component.html"
})
export class SetNotificationsButtonComponent {
    @Input() member!: Member;

    constructor(
        private readonly _membersService: MembersService,
        private readonly _messagesService: MessagesService
    ) {}

    loadingNotifications = false;

    setNotifications() {
        this.loadingNotifications = true;
        this._membersService
            .setNotifications(this.member.id, {
                shouldBeNotified: !this.shouldBeNotified
            })
            .subscribe({
                next: (response: SetNotificationsResponse) => {
                    this._updatePermissions(response);
                    this.loadingNotifications = false;
                    this._messagesService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Updated notifications for ${this.member.name} ${this.member.surname}`
                    });
                },
                error: (error) => {
                    this.loadingNotifications = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error
                    });
                }
            });
    }

    private _updatePermissions(response: SetNotificationsResponse) {
        if (response.shouldBeNotified) {
            this.member.permissions.push("get-notifications");
        } else {
            this.member.permissions = this.member.permissions.filter(
                (permission) => permission !== "get-notifications"
            );
        }
    }

    get shouldBeNotified(): boolean {
        return this.member.permissions.includes("get-notifications");
    }
}
