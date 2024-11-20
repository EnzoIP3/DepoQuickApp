import { CanActivateFn, Router } from "@angular/router";
import UserLogged from "../backend/services/auth/models/user-logged";
import { inject } from "@angular/core";

export const permissionsGuard: CanActivateFn = (route, __) => {
    const user = JSON.parse(localStorage.getItem("user")!) as UserLogged;
    const requiredPermissions = route.data["permissions"] as string[];
    const hasPermissions = requiredPermissions.every((p) =>
        user.roles[user.currentRole].includes(p)
    );

    if (!hasPermissions) {
        const router = inject(Router);
        return router.parseUrl("/");
    }

    return true;
};
