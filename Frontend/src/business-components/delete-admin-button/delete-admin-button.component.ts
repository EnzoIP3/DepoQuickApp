import { CommonModule } from "@angular/common";
import { Component, Input, OnDestroy } from "@angular/core";
import { AdminsService } from "../../backend/services/admins/admins.service";
import User from "../../backend/services/users/models/user";
import { ButtonComponent } from "../../components/button/button.component";
import { UsersService } from "../../backend/services/users/users.service";
import { Subscription } from "rxjs";
import { MessagesService } from "../../backend/services/messages/messages.service";

@Component({
    selector: "app-delete-admin-button",
    standalone: true,
    imports: [CommonModule, ButtonComponent],
    templateUrl: "./delete-admin-button.component.html"
})
export class DeleteAdminButtonComponent implements OnDestroy {
    @Input() user!: User | null;

    constructor(
        private readonly _usersService: UsersService,
        private readonly _adminsService: AdminsService,
        private readonly _messagesService: MessagesService
    ) {}

    private _deleteAdminSubscription: Subscription | null = null;
    private _getUsersSubscription: Subscription | null = null;
    deleting = false;

    deleteAdmin() {
        if (this.user && this.user.roles.includes("Admin")) {
            this.deleting = true;
            this._deleteAdminSubscription = this._adminsService
                .deleteAdmin(this.user.id)
                .subscribe({
                    next: () => {
                        this.deleting = false;
                        this._messagesService.add({
                            severity: "success",
                            summary: "Success",
                            detail: `Admin ${this.user?.name} deleted successfully`
                        });
                        this.user = null;
                        this._getUsersSubscription = this._usersService
                            .getUsers()
                            .subscribe();
                    },
                    error: (error) => {
                        this.deleting = false;
                        this._messagesService.add({
                            severity: "error",
                            summary: "Error",
                            detail: `Failed to delete admin: ${error.message}`
                        });
                    }
                });
        } else {
            this._messagesService.add({
                severity: "warn",
                summary: "Warning",
                detail: "Please select an admin to delete"
            });
        }
    }

    ngOnDestroy() {
        this._deleteAdminSubscription?.unsubscribe();
        this._getUsersSubscription?.unsubscribe();
    }
}
