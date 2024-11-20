import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ButtonComponent } from "../../components/button/button.component";
import Member from "../../backend/services/homes/models/member";
import { MembersService } from "../../backend/services/members/members.service";
import SetNotificationsResponse from "../../backend/services/members/models/set-notifications-response";
import { MessageService } from "primeng/api";

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
        private readonly _messageService: MessageService
    ) {}

    loadingNotifications = false;

    setNotifications() {
        this.loadingNotifications = true;
        this._membersService
            .setNotifications(this.member.id, {
                shouldBeNotified: !this.member.shouldBeNotified
            })
            .subscribe({
                next: (response: SetNotificationsResponse) => {
                    this.member.shouldBeNotified = response.shouldBeNotified;
                    this.loadingNotifications = false;
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Updated notifications for ${this.member.name} ${this.member.surname}`
                    });
                },
                error: (error) => {
                    this.loadingNotifications = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error
                    });
                }
            });
    }
}
