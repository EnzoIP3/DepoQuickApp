import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { MessageService } from "primeng/api";
import { ButtonComponent } from "../../../components/button/button.component";
import { AdminsService } from "../../../backend/services/admins/admins.service";
import User from "../../../backend/services/user/models/user";

@Component({
    selector: "app-delete-admin-button",
    standalone: true,
    imports: [CommonModule, ButtonComponent],
    templateUrl: "./delete-admin-button.component.html"
})
export class DeleteAdminButtonComponent {
    @Input() user!: User | null;

    constructor(
        private readonly _adminsService: AdminsService,
        private readonly _messageService: MessageService
    ) {}

    deleting: boolean = false;

    deleteAdmin() {
        if (this.user && this.user.roles.includes("Admin")) {
            this.deleting = true;
            this._adminsService.deleteAdmin(this.user.id).subscribe({
                next: () => {
                    this.deleting = false;
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Admin ${this.user?.name} deleted successfully`
                    });
                    this.user = null;
                },
                error: (error) => {
                    this.deleting = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: `Failed to delete admin: ${error.message}`
                    });
                }
            });
        } else {
            this._messageService.add({
                severity: "warn",
                summary: "Warning",
                detail: "Please select an admin to delete"
            });
        }
    }
}
