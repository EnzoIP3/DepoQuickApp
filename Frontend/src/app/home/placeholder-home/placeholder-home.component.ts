import { Component } from "@angular/core";
import { AuthService } from "../../../backend/services/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: "app-placeholder-home",
    templateUrl: "./placeholder-home.component.html"
})
export class PlaceholderHomeComponent {
    constructor(
        private _authService: AuthService,
        private _router: Router
    ) {}

    logout(): void {
        this._authService.logout();
        this._router.navigate(["/"]);
    }
}
