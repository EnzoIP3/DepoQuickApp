import { Injectable } from "@angular/core";
import { AuthApiRepositoryService } from "../../repositories/auth-api-repository.service";
import { BehaviorSubject, Observable, tap } from "rxjs";
import UserLogged from "./models/user-logged";
import AuthRequest from "./models/auth-request";

@Injectable({
    providedIn: "root"
})
export class AuthService {
    private readonly _userLogged$ = new BehaviorSubject<UserLogged | null>(
        null
    );

    constructor(private readonly _repository: AuthApiRepositoryService) {}

    private getUserLoggedFromLocalStorage(): UserLogged | null {
        const token = localStorage.getItem("token");

        if (token) {
            const roles = JSON.parse(localStorage.getItem("roles") || "[]");
            return { token, permissions: roles };
        }

        return null;
    }

    get userLogged(): Observable<UserLogged | null> {
        const userLogged = this.getUserLoggedFromLocalStorage();

        if (!this._userLogged$.value && userLogged) {
            this._userLogged$.next(userLogged);
        }

        return this._userLogged$.asObservable();
    }

    public login(authRequest: AuthRequest): Observable<UserLogged> {
        return this._repository.login(authRequest).pipe(
            tap((userLogged) => {
                localStorage.setItem("token", userLogged.token);
                localStorage.setItem(
                    "permissions",
                    JSON.stringify(userLogged.permissions)
                );
                this._userLogged$.next(userLogged);
            })
        );
    }

    public logout(): void {
        localStorage.clear();
        this._userLogged$.next(null);
    }
}
