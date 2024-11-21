import { Component, OnInit } from "@angular/core";
import { DropdownComponent } from "../../components/dropdown/dropdown.component";
import { AuthService } from "../../backend/services/auth/auth.service";
import UserLogged from "../../backend/services/auth/models/user-logged";
import { CommonModule } from "@angular/common";
import { Router } from "@angular/router";

@Component({
    selector: "app-role-dropdown",
    standalone: true,
    imports: [CommonModule, DropdownComponent],
    templateUrl: "./role-dropdown.component.html"
})
export class RoleDropdownComponent implements OnInit {
    constructor(
        private readonly _authService: AuthService,
        private readonly _router: Router
    ) {}

    user: UserLogged | null = null;
    canSwitchRoles = false;
    roles: any[] = [];
    selectedRole: string | null = null;

    ngOnInit() {
        this._authService.userLogged.subscribe((user) => {
            this.user = user;
            if (user) {
                this._checkUserRoleCount(user);
                this._setRoles(user);
                this._setSelectedRole(user);
            }
        });
    }

    private _checkUserRoleCount(user: UserLogged) {
        if (user && Object.keys(user.roles).length > 1) {
            this.canSwitchRoles = true;
        }
    }

    private _setRoles(user: UserLogged) {
        this.roles = Object.keys(user.roles).map((role) => {
            return {
                value: role,
                label: role
            };
        });
    }

    private _setSelectedRole(user: UserLogged) {
        this.selectedRole = user.currentRole;
    }

    onRoleChange(role: string) {
        this._authService.switchCurrentRole(role);
        this._router.navigate(["/"]);
    }
}
