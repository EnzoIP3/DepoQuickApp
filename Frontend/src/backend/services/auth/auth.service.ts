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
        const localStorageData = this.getLocalStorageData();

        if (localStorageData?.token) {
            return localStorageData;
        }

        return null;
    }

    private getLocalStorageData(): UserLogged | null {
        const localStorageDataString = localStorage.getItem("user");

        if (localStorageDataString) {
            try {
                return JSON.parse(localStorageDataString) as UserLogged;
            } catch {
                return null;
            }
        }

        return null;
    }

    private setLocalStorageData(data: UserLogged): void {
        localStorage.setItem("user", JSON.stringify(data));
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
                this.setLocalStorageData(userLogged);
                this._userLogged$.next(userLogged);
            })
        );
    }

    public logout(): void {
        localStorage.clear();
        this._userLogged$.next(null);
    }
}
