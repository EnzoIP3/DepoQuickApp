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
    @Input() loading: boolean = false;
    @Output() rowClick: EventEmitter<any> = new EventEmitter();
    @Output() pageChange: EventEmitter<any> = new EventEmitter();
    @Output() filterChange: EventEmitter<any> = new EventEmitter();

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
