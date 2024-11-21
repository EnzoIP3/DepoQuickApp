import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";
import { BusinessOwnersService } from "../../../backend/services/business-owners/business-owners.service";
import { MessagesService } from "../../../backend/services/messages/messages.service";

@Component({
    selector: "app-add-businessowner-form",
    templateUrl: "./add-business-owner-form.component.html"
})
export class AddBusinessOwnerFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        name: {
            required: { message: "Name is required" }
        },
        surname: {
            required: { message: "Surname is required" }
        },
        email: {
            required: { message: "Email is required" },
            email: { message: "Email format is invalid" }
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
    businessOwnerStatus = { loading: false, success: false, error: null };
    private _addBusinessOwnerSubscription: Subscription | null = null;
    adminForm: any;

    constructor(
        private _formBuilder: FormBuilder,
        private _businessOwnersService: BusinessOwnersService,
        private _messagesService: MessagesService
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
        this.businessOwnerStatus.loading = true;
        this.businessOwnerStatus.error = null;
        this.businessOwnerStatus.success = false;

        this._addBusinessOwnerSubscription = this._businessOwnersService
            .addBusinessOwner(this.addForm.value)
            .subscribe({
                next: () => {
                    this.businessOwnerStatus.loading = false;
                    this.businessOwnerStatus.success = true;
                    this.addForm.reset();
                    this._messagesService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Business Owner created successfully"
                    });
                },
                error: (error) => {
                    this.businessOwnerStatus.loading = false;
                    this.businessOwnerStatus.error = error.message;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._addBusinessOwnerSubscription?.unsubscribe();
    }
}
