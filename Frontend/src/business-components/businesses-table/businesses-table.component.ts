import { Component } from '@angular/core';
import { PaginatorComponent } from '../../components/paginator/paginator.component';
import { TableComponent } from '../../components/table/table.component';
import TableColumn from '../../components/table/models/table-column';
import { Subscription } from 'rxjs';
import PaginationResponse from "../../backend/services/pagination";
import { MessageService } from 'primeng/api';
import Pagination from '../../backend/services/pagination';
import FilterValues from '../../components/table/models/filter-values';
import GetBusinessesResponse from '../../backend/services/businesses/models/business-response';
import Business from '../../backend/services/businesses/models/Business';
import { BusinessesService } from '../../backend/services/businesses/businesses.service';

@Component({
  selector: 'app-businesses-table',
  standalone: true,
  imports: [TableComponent, PaginatorComponent],
  templateUrl: './businesses-table.component.html',
  styles: ``
})
export class BusinessesTableComponent {
  columns: TableColumn[] = [
      {
        field : "name",
        header : "Business Name"
      },
      {
          field: "ownerFullName",
          header: "Owner Name"
      },
      {
          field: "ownerEmail",
          header: "Role"
      },
      {
          field: "rut",
          header: "RUT"
      },
  ];

  filterableColumns: string[] = [
      "name",
      "ownerFullName",
      "RUT"
  ];

  private _businessesSubscription: Subscription | null = null;

  ownerName: any[] = [];
  pagination: PaginationResponse | null = null;
  filters: FilterValues = {};
  loading: boolean = true;
businesses: any;

  constructor(
      private readonly _businessesService: BusinessesService,
      private readonly _messageService: MessageService
  ) {}

  ngOnInit() {
      this._subscribeToBusinesses();
  }

  onPageChange(pagination: Pagination): void {
      this.loading = true;
      this._subscribeToBusinesses({ ...pagination, ...this.filters });
  }

  onFilterChange(filters: FilterValues) {
      this.filters = filters;
      this._subscribeToBusinesses({ ...this.pagination, ...filters });
  }

  ngOnDestroy() {
      this._businessesSubscription?.unsubscribe();
  }

  private _subscribeToBusinesses(queries?: object): void {
    this._businessesSubscription = this._businessesService
        .getBusinesses(queries ? { ...queries } : {})
        .subscribe({
            next: (response: GetBusinessesResponse) => {
                this.ownerName = response.businesses.map((business: Business) => ({
                    ...business,
                    fullName: `${business.ownerName} ${business.ownerSurname}`
                }));
                this.pagination = response.pagination;
                this.loading = false;
            },
            error: (error) => {
                this.loading = false;
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
            }
        });
}
}


