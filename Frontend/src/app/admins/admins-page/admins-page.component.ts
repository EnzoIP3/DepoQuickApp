import { Component } from "@angular/core";
import { MessageService } from "primeng/api";
import { AdminsService } from "../../../backend/services/admins/admins.service";
import User from "../../../backend/services/users/models/user";

@Component({
    selector: "app-admins-page",
    templateUrl: "./admins-page.component.html"
})
export class AdminsPageComponent {
    selectedUser: User | null = null;

    onUserSelected(user: User): void {
        this.selectedUser = user;
    }
}
