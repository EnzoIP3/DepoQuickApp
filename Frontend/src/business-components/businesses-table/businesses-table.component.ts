import { Component, OnInit, OnDestroy } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Router } from "@angular/router";
import Business from "../../backend/services/businesses/models/business";
import PaginationResponse from "../../backend/services/pagination";
import Pagination from "../../backend/services/pagination";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { UsersService } from "../../backend/services/users/users.service";
import { GetBusinessResponse } from "../../backend/services/users/models/get-business-response";
import { AuthService } from "../../backend/services/auth/auth.service";
import { BaseBusinessesTableComponent } from "../base-businesses-table/base-businesses-table.component";

@Component({
    selector: "app-businesses-table",
    standalone: true,
    imports: [BaseBusinessesTableComponent],
    templateUrl: "./businesses-table.component.html"
})
export class BusinessesTableComponent implements OnInit, OnDestroy {
    columns: TableColumn[] = [
        { field: "logo", header: "Logo" },
        { field: "name", header: "Business Name" },
        { field: "ownerName", header: "Owner Name" },
        { field: "ownerEmail", header: "Owner Email" },
        { field: "rut", header: "RUT" }
    ];

    businesses: Business[] = [];

    pagination: PaginationResponse | null = {};

    loading = true;
    private _businessesSubscription: Subscription | null = null;
    private _authSubscription: Subscription | null = null;
    private _userId: string | null = null;

    constructor(
        private readonly _router: Router,
        private readonly _userService: UsersService,
        private readonly _messageService: MessageService,
        private readonly _authService: AuthService
    ) {}

    onPageChange(pagination: Pagination): void {
        this.loading = true;
        this._subscribeToBusinesses({ ...pagination });
    }

    ngOnInit() {
        this._getUserId();
    }

    private _getUserId() {
        this._authSubscription = this._authService.userLogged.subscribe({
            next: (user) => {
                if (user) {
                    this._userId = user.userId;
                    this._subscribeToBusinesses();
                } else {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: "No user is logged in"
                    });
                }
            },
            error: (error) => {
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
            }
        });
    }

    ngOnDestroy() {
        this._businessesSubscription?.unsubscribe();
        this._authSubscription?.unsubscribe();
    }

    onRowClick(business: Business) {
        this._router.navigate(["/businesses", business.rut]);
    }

    private _subscribeToBusinesses(queries?: object): void {
        if (!this._userId) {
            return;
        }

        this._businessesSubscription = this._userService
            .getBusinesses(queries ? { ...queries } : {}, this._userId)
            .subscribe({
                next: (response: GetBusinessResponse) => {
                    this.businesses = response.businesses;
                    this.pagination = response.pagination;
                    this.loading = false;
                },
                error: (error) => {
                    this.loading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }
}
