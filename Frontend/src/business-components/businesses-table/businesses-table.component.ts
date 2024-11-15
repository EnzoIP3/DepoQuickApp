import { Component } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import { Router } from "@angular/router";
import { CommonModule } from "@angular/common";
import Business from "../../backend/services/businesses/models/business";
import { TableComponent } from "../../components/table/table.component";

@Component({
    selector: "app-businesses-table",
    standalone: true,
    imports: [CommonModule, TableComponent],
    templateUrl: "./businesses-table.component.html"
})
export class BusinessesTableComponent {
    columns: TableColumn[] = [
        {
            field: "name",
            header: "Business Name"
        },
        {
            field: "ownerName",
            header: "Owner Name"
        },
        {
            field: "ownerSurname",
            header: "Owner Surname"
        },
        {
            field: "ownerEmail",
            header: "Owner Email"
        },
        {
            field: "rut",
            header: "RUT"
        }
    ];

    // Datos de ejemplo (dummy)
    businesses: Business[] = [
        new Business("1", "Business 1", "John", "Doe", "john.doe@example.com", "12345678"),
        new Business("2", "Business 2", "Jane", "Smith", "jane.smith@example.com", "87654321"),
        new Business("3", "Business 3", "Alice", "Johnson", "alice.johnson@example.com", "13579246"),
        new Business("4", "Business 4", "Bob", "Brown", "bob.brown@example.com", "24681357")
    ];

    loading: boolean = false;  // No necesitamos cargar nada desde un servicio

    constructor(private readonly _router: Router) {}

    onRowClick(business: Business) {
        this._router.navigate(["/businesses", business.id]);  // Ajusta la ruta según tu estructura
    }
}
