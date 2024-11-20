import { Component, EventEmitter, Output } from "@angular/core";
import { TableComponent } from "../../components/table/table.component";
import PaginationResponse from "../../backend/services/pagination";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import { MessageService } from "primeng/api";
import Pagination from "../../backend/services/pagination";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import FilterValues from "../../components/table/models/filter-values";
import { UsersService } from "../../backend/services/user/user.service";
import GetUsersResponse from "../../backend/services/user/models/user-response";
import User from "../../backend/services/user/models/user";
import { Router } from "@angular/router";

@Component({
  selector: 'app-users-table',
  standalone: true,
  imports: [TableComponent, PaginatorComponent],
  templateUrl: './users-table.component.html',
  styles: ``
})
export class UsersTableComponent {
  columns: TableColumn[] = [
      {
          field: "fullName",
          header: "Full Name"
      },
      {
          field: "roles",
          header: "Role"
      },
      {
          field: "createdAt",
          header: "Created"
      },
  ];

  filterableColumns: string[] = [
      "fullName",
      "roles",
  ];

  private _usersSubscription: Subscription | null = null;
  selectedUser: User | null = null;
  @Output() userSelected = new EventEmitter<User>();

  users: any[] = [];
  pagination: PaginationResponse | null = null;
  filters: FilterValues = {};
  loading: boolean = true;

  constructor(
      private readonly _usersService: UsersService,
      private readonly _messageService: MessageService
  ) {}

  ngOnInit() {
      this._subscribeToUsers();
  }

  onRowClick(user: User): void {
    this.selectedUser = user;
    console.log('Usuario seleccionado:', user);
    this.userSelected.emit(user);
  }
  

  onPageChange(pagination: Pagination): void {
      this.loading = true;
      this._subscribeToUsers({ ...pagination, ...this.filters });
  }

  onFilterChange(filters: FilterValues) {
      this.filters = filters;
      this._subscribeToUsers({ ...this.pagination, ...filters });
  }

  ngOnDestroy() {
      this._usersSubscription?.unsubscribe();
  }

  private _subscribeToUsers(queries?: object): void {
      this._usersSubscription = this._usersService
          .getUsers(queries ? { ...queries } : {})
          .subscribe({
              next: (response: GetUsersResponse) => {
                  this.users = response.users.map((user: User) => ({
                      ...user,
                      roles: user.roles.join(", "),
                      fullName: `${user.name} ${user.surname}`}));
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

