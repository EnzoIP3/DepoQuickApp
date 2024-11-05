import { Component } from "@angular/core";
import { AuthService } from "../../backend/services/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: "app-home",
    templateUrl: "./home.component.html"
})
export class HomeComponent {
    permissions: string[] = [];

    constructor(
        private _authService: AuthService,
        private _router: Router
    ) {}

    logout() {
        this._authService.logout();
        this._router.navigate(["/login"]);
    }

    ngOnInit() {
        this._authService.userLogged.subscribe((userLogged) => {
            if (userLogged) {
                this.permissions = userLogged.permissions;
            }
        });
    }
}
