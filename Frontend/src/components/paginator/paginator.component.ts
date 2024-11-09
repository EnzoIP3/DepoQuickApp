import { Component, EventEmitter, Input, Output } from "@angular/core";
import { PaginatorModule } from "primeng/paginator";
import Pagination from "./models/pagination";

@Component({
    selector: "app-paginator",
    standalone: true,
    imports: [PaginatorModule],
    templateUrl: "./paginator.component.html"
})
export class PaginatorComponent {
    @Input() currentPage: number = 1;
    @Input() pageSize: number = 10;
    @Input() totalPages: number = 0;
    @Output() pageChange = new EventEmitter<Pagination>();

    get totalRecords(): number {
        return this.totalPages * this.pageSize;
    }

    onPageChange(event: any): void {
        const newPage = Math.floor(event.first / event.rows) + 1;
        this.pageChange.emit({
            page: newPage,
            pageSize: event.rows
        });
    }
}
