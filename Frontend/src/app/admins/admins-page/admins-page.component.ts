import { Component } from '@angular/core';
import { MessageService } from "primeng/api";
import { AdminsService } from '../../../backend/services/admins/admins.service';
import User from '../../../backend/services/user/models/user';

@Component({
  selector: 'app-admins-page',
  templateUrl: './admins-page.component.html',
  styles: ``
})
export class AdminsPageComponent {
  selectedUser: User | null = null;

  constructor(
    private readonly _adminsService: AdminsService,
    private readonly _messageService: MessageService
  ) {}

  onUserSelected(user: User): void {
    this.selectedUser = user;
    console.log('Usuario seleccionado:', user);
  }

  deleteAdmin() {
    if (this.selectedUser && this.selectedUser.roles.includes("Admin")) {
      this._adminsService.deleteAdmin(this.selectedUser.id).subscribe({
        next: () => {
          this._messageService.add({
            severity: "success",
            summary: "Success",
            detail: "Admin deleted successfully",
          });
          this.selectedUser = null;
        },
        error: (error) => {
          this._messageService.add({
            severity: "error",
            summary: "Error",
            detail: `Error deleting admin: ${error.message}`,
          });
        },
      });
    } else {
      this._messageService.add({
        severity: "warn",
        summary: "Warning",
        detail: "Please select an admin to delete",
      });
    }
  }
}