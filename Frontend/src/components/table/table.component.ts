import { Component, Input, Output, EventEmitter } from "@angular/core";
import { TableModule } from "primeng/table";
import { CommonModule } from "@angular/common";
import { SkeletonModule } from "primeng/skeleton";
import TableColumn from "./models/table-column";
import { InputTextModule } from "primeng/inputtext";
import { FormsModule } from "@angular/forms";

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
    private static readonly DEFAULT_LOADING_ROWS = 5;

    @Input() columns: TableColumn[] = [];
    @Input() data: any[] = [];
    @Input() loading: boolean = false;

    public readonly loadingArray = new Array(
        TableComponent.DEFAULT_LOADING_ROWS
    );
}
