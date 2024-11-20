import { Component, OnInit } from "@angular/core";
import { ButtonComponent } from "../../components/button/button.component";
import { AuthService } from "../../backend/services/auth/auth.service";
import { UsersService } from "../../backend/services/users/users.service";
import UserLogged from "../../backend/services/auth/models/user-logged";
import { MessageService } from "primeng/api";
import { CommonModule } from "@angular/common";
import { DialogComponent } from "../../components/dialog/dialog.component";

@Component({
    selector: "app-add-home-owner-role-button",
    standalone: true,
    imports: [CommonModule, ButtonComponent, DialogComponent],
    templateUrl: "./add-home-owner-role-button.component.html"
})
export class AddHomeOwnerRoleButtonComponent implements OnInit {
    constructor(
        private readonly _authService: AuthService,
        private readonly _usersService: UsersService,
        private readonly _messageService: MessageService
    ) {}

    user: UserLogged | null = null;
    loading = false;
    canAddHomeOwnerRole = false;
    showConfirmationDialog = false;

    ngOnInit() {
        this._authService.userLogged.subscribe((user) => {
            this.user = user;
            if (user) {
                this._checkIfCanAddHomeOwnerRole(user);
            }
        });
    }

    private _checkIfCanAddHomeOwnerRole(user: UserLogged) {
        const isNotHomeOwner = !Object.keys(user.roles).includes("HomeOwner");
        const isAdmin = Object.keys(user.roles).includes("Admin");
        this.canAddHomeOwnerRole = isNotHomeOwner && isAdmin;
    }

    addHomeOwnerRole() {
        this.loading = true;
        this._usersService.addHomeOwnerRole().subscribe({
            next: (response) => {
                this._authService.setRoles(response.roles);
                this.loading = false;
            },
            error: (error) => {
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
                this.loading = false;
            }
        });
    }
}
