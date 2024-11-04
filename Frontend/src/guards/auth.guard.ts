import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const authGuard: CanActivateFn = (route, state) => {
    const notLoggedIn = localStorage.getItem("token") === null;

    if (notLoggedIn) {
        const router = inject(Router);
        return router.parseUrl("/login");
    }

    return true;
};
