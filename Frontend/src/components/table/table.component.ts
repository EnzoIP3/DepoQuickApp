import { Component, Input, Output, EventEmitter, TemplateRef } from "@angular/core";
import { TableModule } from "primeng/table";
import { CommonModule } from "@angular/common";
import { SkeletonModule } from "primeng/skeleton";
import TableColumn from "./models/table-column";
import { InputTextModule } from "primeng/inputtext";
import { FormsModule } from "@angular/forms";
import { PaginatorModule } from "primeng/paginator";
import FilterValues from "./models/filter-values";
import { ButtonModule } from "primeng/button";

@Component({
    selector: "app-table",
    standalone: true,
    imports: [
        TableModule,
        CommonModule,
        SkeletonModule,
        InputTextModule,
        FormsModule,
        PaginatorModule,
        ButtonModule
    ],
    templateUrl: "./table.component.html"
})
export class TableComponent {
    private static readonly DEFAULT_LOADING_ROWS = 5;

    @Input() columns: TableColumn[] = [];
    @Input() data: any[] = [];
    @Input() loading: boolean = false;
    @Input() clickableRows: boolean = false;
    @Input() filterableColumns: string[] = [];
    @Input() customTemplates: { [key: string]: TemplateRef<any> } = {};
    @Output() rowClick = new EventEmitter<any>();
    @Output() filter = new EventEmitter<FilterValues>();

    filterValues: FilterValues = {};
    visibleFilters: string[] = [];

    get loadingArray() {
        return new Array(
            this.columns.length || TableComponent.DEFAULT_LOADING_ROWS
        );
    }

    handleRowClick(row: any) {
        this.rowClick.emit(row.data);
    }

    handleFilterChange(event: any, field: string) {
        const value = event.target.value;
        this.filterValues[field] = value;
    }

    handleFilterSubmit() {
        this.filter.emit(this.filterValues);
    }

    toggleFilterVisibility(field: string) {
        if (this.visibleFilters.includes(field)) {
            this.visibleFilters = this.visibleFilters.filter(
                (filter) => filter !== field
            );
        } else {
            this.visibleFilters.push(field);
        }
    }

    isFilterVisible(field: string) {
        return this.visibleFilters.includes(field);
    }
}
