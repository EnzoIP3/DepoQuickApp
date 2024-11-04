import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders
} from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { Observable, catchError, retry, throwError } from "rxjs";

export interface RequestConfig {
    extraResource?: string;
    query?: string;
}

export default abstract class ApiRepository {
    private static readonly RETRY_ATTEMPTS = 3;
    private static readonly AUTH_TOKEN_KEY = "token";
    private static readonly DEFAULT_ACCEPT = "application/json";
    private static readonly DEFAULT_ERROR_MESSAGE =
        "Something went wrong, please try again later.";

    protected readonly fullEndpoint: string;

    protected get headers() {
        return {
            headers: new HttpHeaders({
                accept: ApiRepository.DEFAULT_ACCEPT,
                Authorization: this.getAuthToken()
            })
        };
    }

    constructor(
        protected readonly apiUrl: string,
        protected readonly endpoint: string,
        protected readonly http: HttpClient
    ) {
        this.fullEndpoint = this.buildFullEndpoint(apiUrl, endpoint);
    }

    protected get<T>(config: RequestConfig = {}): Observable<T> {
        const url = this.buildRequestUrl(config);
        return this.executeRequest(() => this.http.get<T>(url, this.headers));
    }

    protected post<T>(body: any, extraResource = ""): Observable<T> {
        const url = this.buildRequestUrl({ extraResource });
        return this.executeRequest(() =>
            this.http.post<T>(url, body, this.headers)
        );
    }

    private buildFullEndpoint(apiOrigin: string, endpoint: string): string {
        return `${apiOrigin}/${endpoint}`;
    }

    private buildRequestUrl({
        extraResource = "",
        query = ""
    }: RequestConfig): string {
        const formattedExtra = extraResource ? `/${extraResource}` : "";
        const formattedQuery = query ? `?${query}` : "";
        return `${this.fullEndpoint}${formattedExtra}${formattedQuery}`;
    }

    private getAuthToken(): string {
        return localStorage.getItem(ApiRepository.AUTH_TOKEN_KEY) ?? "";
    }

    private executeRequest<T>(requestFn: () => Observable<T>): Observable<T> {
        return requestFn().pipe(
            retry(ApiRepository.RETRY_ATTEMPTS),
            catchError(this.handleError)
        );
    }

    protected handleError(error: HttpErrorResponse) {
        let errorMessage = ApiRepository.DEFAULT_ERROR_MESSAGE;
        let isServerError = "message" in error.error;

        if (isServerError) {
            errorMessage = error.error.message;
        }

        return throwError(() => new Error(errorMessage));
    }
}
