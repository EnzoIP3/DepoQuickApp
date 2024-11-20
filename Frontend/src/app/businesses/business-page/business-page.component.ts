import { Component, OnDestroy, OnInit } from "@angular/core";
import { ActivatedRoute } from "@angular/router";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import Business from "../../../backend/services/businesses/models/business";
import Pagination from "../../../backend/services/pagination";
import { UsersService } from "../../../backend/services/users/users.service";
import PaginationResponse from "../../../backend/services/pagination";
import { AuthService } from "../../../backend/services/auth/auth.service";

@Component({
    selector: "app-business-page",
    templateUrl: "./business-page.component.html",
    styles: []
})
export class BusinessPageComponent implements OnInit, OnDestroy {
    title = "Unnamed business";
    businesses: Business[] = [];
    pagination: PaginationResponse | null = {};
    public businessId = "";
    private _userId: string | null = null;
    private _businessesSubscription: Subscription | null = null;
    private _authSubscription: Subscription | null = null;

    constructor(
        private readonly _route: ActivatedRoute,
        private readonly _userService: UsersService,
        private readonly _authService: AuthService,
        private readonly _messageService: MessageService
    ) {
        this._setBusinessId();
    }

    ngOnInit(): void {
        this._getUserId();
    }

    private _setBusinessId(): void {
        this._route.params.subscribe((params) => {
            this.businessId = params["id"];
        });
    }

    private _getUserId(): void {
        this._authSubscription = this._authService.userLogged.subscribe({
            next: (user) => {
                if (user) {
                    this._userId = user.userId;
                    this._fetchBusinesses();
                } else {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: "No user is logged in"
                    });
                }
            },
            error: () => {
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: "Failed to fetch user information"
                });
            }
        });
    }

    private _fetchBusinesses(
        paginationParams: Pagination = { page: 1, pageSize: 10 }
    ): void {
        if (!this._userId) {
            return;
        }

        this._businessesSubscription = this._userService
            .getBusiness(paginationParams, this._userId)
            .subscribe({
                next: (response) => {
                    this.businesses = response.businesses.filter(
                        (business) => business.rut === this.businessId
                    );

                    if (this.businesses.length > 0) {
                        this.title = this.businesses[0].name;
                    } else {
                        this.title = "Business not found";
                    }

                    this.pagination = response.pagination;
                },
                error: (error) => {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error fetching businesses",
                        detail: error.message
                    });
                }
            });
    }

    onPageChange(paginationParams: Pagination): void {
        this._fetchBusinesses(paginationParams);
    }

    ngOnDestroy(): void {
        this._businessesSubscription?.unsubscribe();
        this._authSubscription?.unsubscribe();
    }
}
