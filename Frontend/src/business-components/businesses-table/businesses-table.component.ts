import { Component } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Router } from "@angular/router";
import Business from "../../backend/services/businesses/models/business";
import { TableComponent } from "../../components/table/table.component";
import PaginationResponse from "../../backend/services/pagination";
import Pagination from "../../backend/services/pagination";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { BusinessesService } from "../../backend/services/businesses/businesses.service";
import { UsersService } from "../../backend/services/users/users.service";
import { GetBusinessResponse } from "../../backend/services/users/models/get-business-response";
import { AuthService } from "../../backend/services/auth/auth.service";

@Component({
    selector: "app-businesses-table",
    standalone: true,
    imports: [TableComponent, PaginatorComponent],
    templateUrl: "./businesses-table.component.html"
})
export class BusinessesTableComponent {
    columns: TableColumn[] = [
        { field: "name", header: "Business Name" },
        { field: "ownerName", header: "Owner Name" },
        { field: "ownerSurname", header: "Owner Surname" },
        { field: "ownerEmail", header: "Owner Email" },
        { field: "rut", header: "RUT" }
    ];

    businesses: Business[] = [
        new Business("Business 1", "John", "Doe", "john.doe@example.com", "12345678"),
        new Business("Business 2", "Jane", "Smith", "jane.smith@example.com", "87654321"),
        new Business("Business 3", "Alice", "Johnson", "alice.johnson@example.com", "13579246"),
        new Business("Business 4", "Bob", "Brown", "bob.brown@example.com", "24681357")
    ];

    pagination: PaginationResponse | null = {};

    loading: boolean = true;
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
                    detail: "Failed to fetch user information"
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
            .getBusiness(queries ? { ...queries } : {}, this._userId)
            .subscribe({
                next: (response: GetBusinessResponse) => {
                    this.businesses = response.businesses.map(business => new Business(
                        business.name,
                        business.ownerName,
                        business.ownerSurname,
                        business.ownerEmail,
                        business.rut
                    ));
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
