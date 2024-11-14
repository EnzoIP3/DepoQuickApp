import { Injectable } from "@angular/core";
import Route from "../../../components/sidebar/models/route";
import UserLogged from "../auth/models/user-logged";
import { UnauthenticatedRoutes, AuthenticatedRoutes } from "./constants/routes";

@Injectable({
    providedIn: "root"
})
export class RouteService {
    public getRoutes(user: UserLogged | null): Route[] {
        if (user) {
            return this._getAuthenticatedRoutes(
                AuthenticatedRoutes,
                user.permissions
            );
        }
        return UnauthenticatedRoutes;
    }

    private _getAuthenticatedRoutes(
        routes: Route[],
        userPermissions: string[]
    ): Route[] {
        return routes.filter((route) =>
            this._isRouteAccessible(route, userPermissions)
        );
    }

    private _isRouteAccessible(
        route: Route,
        userPermissions: string[]
    ): boolean {
        if (!this._hasRequiredPermission(route, userPermissions)) {
            return false;
        }

        if (this._hasSubRoutes(route)) {
            return this._processSubRoutes(route, userPermissions);
        }

        return true;
    }

    private _hasRequiredPermission(
        route: Route,
        userPermissions: string[]
    ): boolean {
        return !route.permission || userPermissions.includes(route.permission);
    }

    private _hasSubRoutes(route: Route): boolean {
        return Boolean(route.items && route.items.length > 0);
    }

    private _processSubRoutes(
        route: Route,
        userPermissions: string[]
    ): boolean {
        route.items = this._getAuthenticatedRoutes(
            route.items!,
            userPermissions
        );
        return route.items.length > 0;
    }
}
