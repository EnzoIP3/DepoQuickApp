import { Component, Input, Output, EventEmitter } from "@angular/core";
import { TableModule } from "primeng/table";
import { CommonModule } from "@angular/common";
import { SkeletonModule } from "primeng/skeleton";
import TableColumn from "./models/table-column";
import { InputTextModule } from "primeng/inputtext";
import { FormsModule } from "@angular/forms";
import { PaginatorModule } from "primeng/paginator";

@Component({
    selector: "app-table",
    standalone: true,
    imports: [
        TableModule,
        CommonModule,
        SkeletonModule,
        InputTextModule,
        FormsModule,
        PaginatorModule
    ],
    templateUrl: "./table.component.html"
})
export class TableComponent {
    private static readonly DEFAULT_LOADING_ROWS = 5;

    @Input() columns: TableColumn[] = [];
    @Input() data: any[] = [];
    @Input() loading: boolean = false;
    @Input() clickableRows: boolean = false;
    @Output() rowClick = new EventEmitter<any>();

    get loadingArray() {
        return new Array(
            this.columns.length || TableComponent.DEFAULT_LOADING_ROWS
        );
    }

    handleRowClick(row: any) {
        this.rowClick.emit(row.data);
    }
}
