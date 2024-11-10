import { Component, EventEmitter, Input, Output } from "@angular/core";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import { AuthService } from "../../backend/services/auth/auth.service";
import { Subscription } from "rxjs";
import UserLogged from "../../backend/services/auth/models/user-logged";
import { Router } from "@angular/router";
import Route from "../../components/sidebar/models/route";

@Component({
    selector: "app-permission-sidebar",
    standalone: true,
    imports: [SidebarComponent],
    templateUrl: "./permission-sidebar.component.html"
})
export class PermissionSidebarComponent {
    @Input() sidebarVisible = false;
    @Output() sidebarVisibleChange = new EventEmitter<boolean>();

    routes: Route[] = [];

    private _routesSubscription: Subscription | null = null;

    private readonly authenticatedRoutes: Route[] = [
        {
            label: "Home",
            icon: "pi pi-home",
            routerLink: ["/home"]
        },
        {
            label: "Logout",
            icon: "pi pi-sign-out",
            command: () => this.handleLogout()
        }
    ];

    private readonly unauthenticatedRoutes: Route[] = [
        {
            label: "Login",
            icon: "pi pi-sign-in",
            routerLink: ["/login"]
        },
        {
            label: "Register",
            icon: "pi pi-user-plus",
            routerLink: ["/register"]
        }
    ];

    constructor(
        private readonly _authService: AuthService,
        private readonly _router: Router
    ) {}

    ngOnInit() {
        this._routesSubscription = this._authService.userLogged.subscribe(
            (user) => {
                this.routes = this.filterRoutes(user);
            }
        );
    }

    private filterRoutes(user: UserLogged | null): Route[] {
        if (user) {
            return this.authenticatedRoutes.filter(
                (route) =>
                    !route.permission ||
                    user.permissions.includes(route.permission)
            );
        } else {
            return this.unauthenticatedRoutes;
        }
    }

    private handleLogout() {
        this._authService.logout();
        this._router.navigate(["/login"]);
    }

    ngOnDestroy() {
        this._routesSubscription?.unsubscribe();
    }

    handleHide() {
        this.sidebarVisible = false;
        this.sidebarVisibleChange.emit(this.sidebarVisible);
    }
}
