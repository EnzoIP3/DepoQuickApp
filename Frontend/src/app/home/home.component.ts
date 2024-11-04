import { Component } from "@angular/core";
import { AuthService } from "../../backend/services/auth/auth.service";
import { Router } from "@angular/router";

@Component({
    selector: "app-home",
    templateUrl: "./home.component.html"
})
export class HomeComponent {
    constructor(
        private _authService: AuthService,
        private _router: Router
    ) {}

    logout() {
        this._authService.logout();
        this._router.navigate(["/login"]);
    }
}
