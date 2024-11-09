import {
    HttpClient,
    HttpErrorResponse,
    HttpHeaders
} from "@angular/common/http";
import { Observable, catchError, retry, throwError } from "rxjs";

export interface RequestConfig {
    extraResource?: string;
    queries?: Record<string, any>;
}

export default abstract class ApiRepository {
    private static readonly RETRY_ATTEMPTS = 3;
    private static readonly DEFAULT_ACCEPT = "application/json";
    private static readonly DEFAULT_ERROR_MESSAGE =
        "Something went wrong, please try again later.";
    private static readonly DEFAULT_EXPIRED_TOKEN_MESSAGE =
        "Your session has expired, please reload the page";

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
        queries = {}
    }: RequestConfig): string {
        const formattedQueries = this.formatQueries(queries);
        const formattedExtra = extraResource ? `/${extraResource}` : "";
        const queryString = formattedQueries ? `?${formattedQueries}` : "";
        return `${this.fullEndpoint}${formattedExtra}${queryString}`;
    }

    private formatQueries(queries: Record<string, any>): string {
        return Object.entries(queries)
            .map(
                ([key, value]) =>
                    `${encodeURIComponent(key)}=${encodeURIComponent(value)}`
            )
            .join("&");
    }

    private getAuthToken(): string {
        return localStorage.getItem("token") ?? "";
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
        let isExpiredTokenError =
            error.status === 401 && localStorage.getItem("token");

        if (isServerError) {
            errorMessage = error.error.message;
        }

        if (isExpiredTokenError) {
            errorMessage = ApiRepository.DEFAULT_EXPIRED_TOKEN_MESSAGE;
            localStorage.clear();
        }

        return throwError(() => new Error(errorMessage));
    }
}
