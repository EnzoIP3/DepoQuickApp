import { Injectable } from "@angular/core";
import { AuthApiRepositoryService } from "../../repositories/auth-api-repository.service";
import { BehaviorSubject, Observable, tap } from "rxjs";
import UserLogged from "./models/user-logged";
import AuthRequest from "./models/auth-request";
import AuthResponse from "./models/auth-response";

@Injectable({
    providedIn: "root"
})
export class AuthService {
    private readonly _userLogged$ = new BehaviorSubject<UserLogged | null>(
        null
    );

    constructor(private readonly _repository: AuthApiRepositoryService) {}

    private _getUserLoggedFromLocalStorage(): UserLogged | null {
        const localStorageData = this._getLocalStorageData();

        if (localStorageData?.token) {
            return localStorageData;
        }

        return null;
    }

    private _getLocalStorageData(): UserLogged | null {
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

    private _setLocalStorageData(data: UserLogged): void {
        localStorage.setItem("user", JSON.stringify(data));
    }

    get userLogged(): Observable<UserLogged | null> {
        const userLogged = this._getUserLoggedFromLocalStorage();

        if (!this._userLogged$.value && userLogged) {
            this._userLogged$.next(userLogged);
        }

        return this._userLogged$.asObservable();
    }

    public login(authRequest: AuthRequest): Observable<AuthResponse> {
        return this._repository.login(authRequest).pipe(
            tap((response) => {
                const userLogged: UserLogged = this._buildUserLogged(response);
                this._setLocalStorageData(userLogged);
                this._userLogged$.next(userLogged);
            })
        );
    }

    private _buildUserLogged(
        response: AuthResponse,
        role?: string
    ): UserLogged {
        role ??= Object.keys(response.roles)[0];
        return {
            userId: response.userId,
            token: response.token,
            roles: response.roles,
            currentRole: Object.keys(response.roles)[0]
        };
    }

    public logout(): void {
        localStorage.clear();
        this._userLogged$.next(null);
    }

    public setRoles(roles: Record<string, string[]>): void {
        const userLogged = this._userLogged$.value;
        if (userLogged) {
            const newUserLogged = {
                ...userLogged,
                roles: {
                    ...userLogged.roles,
                    ...roles
                }
            };
            this._setLocalStorageData(newUserLogged);
            this._userLogged$.next(newUserLogged);
        }
    }

    public switchCurrentRole(role: string): void {
        const userLogged = this._userLogged$.value;
        if (userLogged) {
            const newUserLogged = {
                ...userLogged,
                currentRole: role
            };
            this._setLocalStorageData(newUserLogged);
            this._userLogged$.next(newUserLogged);
        }
    }
}
