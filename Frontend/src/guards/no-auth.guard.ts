import { inject } from "@angular/core";
import { CanActivateFn, Router } from "@angular/router";

export const noAuthGuard: CanActivateFn = (route, state) => {
    const loggedIn = localStorage.getItem("token") !== null;

    if (loggedIn) {
        const router = inject(Router);
        return router.parseUrl("/home");
    }

    return true;
};
