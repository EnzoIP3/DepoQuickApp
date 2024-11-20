import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";
import { AdminsService } from "../../../backend/services/admins/admins.service";
import { MessageService } from "primeng/api";

@Component({
    selector: "app-add-admin-form",
    templateUrl: "./add-admin-form.component.html"
})
export class AddAdminFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        name: {
            required: { message: "Name is required" }
        },
        surname: {
            required: { message: "Surname is required" }
        },
        email: {
            required: { message: "Email is required" },
            email: {
                message: "Email format is invalid"
            }
        },
        password: {
            required: {
                message: "Password is required"
            },
            minlength: {
                message: "Password must be at least 8 characters long"
            }
        }
    };

    addForm!: FormGroup;
    adminStatus = { loading: false, success: false, error: null };
    private _addAdminSubscription: Subscription | null = null;
    adminForm: any;

    constructor(
        private _formBuilder: FormBuilder,
        private _adminsService: AdminsService,
        private _messageService: MessageService
    ) {}

    ngOnInit() {
        this.addForm = this._formBuilder.group({
            name: ["", [Validators.required]],
            surname: ["", [Validators.required]],
            email: ["", [Validators.required, Validators.email]],
            password: ["", [Validators.required, Validators.minLength(8)]]
        });
    }

    onSubmit() {
        this.adminStatus.loading = true;
        this.adminStatus.error = null;
        this.adminStatus.success = false;

        this._addAdminSubscription = this._adminsService
            .addAdmin(this.addForm.value)
            .subscribe({
                next: () => {
                    this.adminStatus.loading = false;
                    this.adminStatus.success = true;
                    this.addForm.reset();
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Admin created successfully"
                    });
                },
                error: (error) => {
                    this.adminStatus.loading = false;
                    this.adminStatus.error = error.message;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._addAdminSubscription?.unsubscribe();
    }
}
