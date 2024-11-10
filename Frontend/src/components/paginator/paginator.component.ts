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
    @Input() currentPage: number | undefined = 1;
    @Input() pageSize: number | undefined = 10;
    @Input() totalPages: number | undefined = 0;
    @Output() pageChange = new EventEmitter<Pagination>();

    get totalRecords(): number {
        if (!this.totalPages || !this.pageSize) {
            return 0;
        }
        return this.totalPages * this.pageSize;
    }

    onPageChange(event: any): void {
        const newPage = Math.floor(event.first / event.rows) + 1;
        if (newPage === this.currentPage && event.rows === this.pageSize) {
            return;
        }
        this.pageChange.emit({
            page: newPage,
            pageSize: event.rows
        });
    }
}
