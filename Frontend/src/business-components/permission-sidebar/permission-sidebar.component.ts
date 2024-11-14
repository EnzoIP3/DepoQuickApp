import {
    Component,
    OnInit,
    OnDestroy,
    Input,
    Output,
    EventEmitter
} from "@angular/core";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { AuthService } from "../../backend/services/auth/auth.service";
import { RouteService } from "../../backend/services/routes/routes.service";
import { SidebarComponent } from "../../components/sidebar/sidebar.component";
import Route from "../../components/sidebar/models/route";

@Component({
    selector: "app-permission-sidebar",
    standalone: true,
    imports: [SidebarComponent],
    templateUrl: "./permission-sidebar.component.html"
})
export class PermissionSidebarComponent implements OnInit, OnDestroy {
    @Input() sidebarVisible = false;
    @Output() sidebarVisibleChange = new EventEmitter<boolean>();

    routes: Route[] = [];
    private _routesSubscription: Subscription | null = null;

    constructor(
        private readonly _authService: AuthService,
        private readonly _router: Router,
        private readonly _routeService: RouteService
    ) {}

    ngOnInit() {
        this._routesSubscription = this._authService.userLogged.subscribe(
            (user) => {
                this.routes = this._routeService.getRoutes(user);
                if (user) {
                    this._addLogoutRoute();
                }
            }
        );
    }

    private _addLogoutRoute() {
        this.routes.push({
            label: "Session",
            items: [
                {
                    label: "Logout",
                    icon: "pi pi-sign-out",
                    command: () => this._handleLogout()
                }
            ]
        });
    }

    private _handleLogout() {
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
