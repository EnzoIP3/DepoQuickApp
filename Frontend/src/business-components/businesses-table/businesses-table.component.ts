import { Component } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Router } from "@angular/router";
import Business from "../../backend/services/businesses/models/business";
import { TableComponent } from "../../components/table/table.component";
import PaginationResponse from "../../backend/services/pagination";
import Pagination from "../../backend/services/pagination";
import { PaginatorComponent } from "../../components/paginator/paginator.component";

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
        new Business("1", "Business 1", "John", "Doe", "john.doe@example.com", "12345678"),
        new Business("2", "Business 2", "Jane", "Smith", "jane.smith@example.com", "87654321"),
        new Business("3", "Business 3", "Alice", "Johnson", "alice.johnson@example.com", "13579246"),
        new Business("4", "Business 4", "Bob", "Brown", "bob.brown@example.com", "24681357")
    ];

    pagination: PaginationResponse | null = {
        page: 1,
        pageSize: 10,
        totalPages: 2
    };

    loading: boolean = false;

    constructor(private readonly _router: Router) {}

    onPageChange(pagination: Pagination): void {
        console.log("Page changed: ", pagination);
        this.loading = true;
        this._loadDummyData(pagination);
    }

    private _loadDummyData(pagination: Pagination = { page: 1, pageSize: 10 }): void {
        console.log("Loading businesses with pagination: ", pagination);
        this.loading = false;
        console.log("Simulated businesses data: ", this.businesses);
        console.log("Simulated pagination: ", this.pagination);
    }

    onRowClick(business: Business) {
        this._router.navigate(["/businesses", business.id]);
    }
}
