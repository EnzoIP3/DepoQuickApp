import { Component, EventEmitter, Input, Output } from "@angular/core";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { TableComponent } from "../../components/table/table.component";
import TableColumn from "../../components/table/models/table-column";

@Component({
    selector: "app-base-businesses-table",
    standalone: true,
    imports: [TableComponent, PaginatorComponent, AvatarComponent],
    templateUrl: "./base-businesses-table.component.html"
})
export class BaseBusinessesTableComponent {
    @Input() columns: TableColumn[] = [];
    @Input() businesses: any[] = [];
    @Input() filterableColumns: string[] = [];
    @Input() pagination: any = {};
    @Input() loading = false;
    @Output() rowClick = new EventEmitter<any>();
    @Output() pageChange = new EventEmitter<any>();
    @Output() filterChange = new EventEmitter<any>();

    onRowClick(row: any) {
        this.rowClick.emit(row);
    }

    onPageChange(event: any) {
        this.pageChange.emit(event);
    }

    onFilterChange(event: any) {
        this.filterChange.emit(event);
    }
}
