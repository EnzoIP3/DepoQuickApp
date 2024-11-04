import { HttpInterceptorFn } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { HttpErrorResponse } from "@angular/common/http";
import { throwError } from "rxjs";
import { catchError } from "rxjs/operators";

export const tokenExpirationInterceptor: HttpInterceptorFn = (req, next) => {
    return next(req).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === 401 && localStorage.getItem("token")) {
                localStorage.removeItem("token");
                localStorage.removeItem("roles");
                const router = inject(Router);
                router.navigate(["/login"]);
            }
            return throwError(() => error);
        })
    );
};
