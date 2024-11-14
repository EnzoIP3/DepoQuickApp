import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const noAuthGuard: CanActivateFn = (_, __) => {
    const loggedIn = localStorage.getItem("user") !== null;

    if (loggedIn) {
        const router = inject(Router);
        return router.parseUrl("/devices");
    }

    return true;
};
