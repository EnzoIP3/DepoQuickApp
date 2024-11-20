import { Component, OnInit, OnDestroy } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import PaginationResponse from "../../backend/services/pagination";
import { MessageService } from "primeng/api";
import Pagination from "../../backend/services/pagination";
import { FilterValues } from "../../components/table/models/filter-values";
import GetBusinessesResponse from "../../backend/services/businesses/models/business-response";
import { BusinessesService } from "../../backend/services/businesses/businesses.service";
import Business from "../../backend/services/businesses/models/business";
import { BaseBusinessesTableComponent } from "../base-businesses-table/base-businesses-table.component";

@Component({
    selector: "app-admin-businesses-table",
    standalone: true,
    imports: [BaseBusinessesTableComponent],
    templateUrl: "./admin-businesses-table.component.html"
})
export class AdminBusinessesTableComponent implements OnInit, OnDestroy {
    columns: TableColumn[] = [
        {
            field: "logo",
            header: "Logo"
        },
        {
            field: "name",
            header: "Business Name"
        },
        {
            field: "ownerName",
            header: "Owner Name"
        },
        {
            field: "ownerEmail",
            header: "Role"
        },
        {
            field: "rut",
            header: "RUT"
        }
    ];

    filterableColumns: string[] = ["name", "ownerName"];

    private _businessesSubscription: Subscription | null = null;

    pagination: PaginationResponse | null = null;
    filters: FilterValues = {};
    loading = true;
    businesses: Business[] = [];

    constructor(
        private readonly _businessesService: BusinessesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this._subscribeToBusinesses();
    }

    onPageChange(pagination: Pagination): void {
        this.loading = true;
        this._subscribeToBusinesses({ ...pagination, ...this.filters });
    }

    onFilterChange(filters: FilterValues) {
        this.filters = filters;
        this._subscribeToBusinesses({ ...this.pagination, ...filters });
    }

    ngOnDestroy() {
        this._businessesSubscription?.unsubscribe();
    }

    private _subscribeToBusinesses(queries?: object): void {
        this._businessesSubscription = this._businessesService
            .getBusinesses(queries ? { ...queries } : {})
            .subscribe({
                next: (response: GetBusinessesResponse) => {
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
