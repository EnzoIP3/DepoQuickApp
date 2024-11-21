import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const authGuard: CanActivateFn = (_, __) => {
    const notLoggedIn = localStorage.getItem("user") === null;

    if (notLoggedIn) {
        const router = inject(Router);
        return router.parseUrl("/login");
    }

    return true;
};
