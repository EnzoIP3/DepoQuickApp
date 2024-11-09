import { Component, Input, Output, EventEmitter } from "@angular/core";
import { TableFilterEvent, TableModule } from "primeng/table";
import { CommonModule } from "@angular/common";
import { SkeletonModule } from "primeng/skeleton";
import TableColumn from "./models/table-column";
import { InputTextModule } from "primeng/inputtext";
import { FormsModule } from "@angular/forms";
import TableFilter from "./models/table-filters";

@Component({
    selector: "app-table",
    standalone: true,
    imports: [
        TableModule,
        CommonModule,
        SkeletonModule,
        InputTextModule,
        FormsModule
    ],
    templateUrl: "./table.component.html"
})
export class TableComponent {
    @Input() columns: TableColumn[] = [];
    @Input() data: any[] = [];
    @Input() loading: boolean = false;

    @Output() filter = new EventEmitter<TableFilter[]>();

    public readonly loadingArray = new Array(3);

    public applyFilter(event: TableFilterEvent): void {
        const filters = event.filters;
        const tableFilters: TableFilter[] = Object.entries(filters ?? {}).map(
            ([field, filterArray]) => ({
                field,
                value: (filterArray as any[])[0].value
            })
        );

        this.filter.emit(tableFilters);
    }
}
