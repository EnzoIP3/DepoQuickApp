import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { BusinessesService } from "../../../backend/services/businesses/businesses.service";
import CreateBusinessResponse from "../../../backend/services/businesses/models/create-business-response";
import { DeviceValidatorsService } from "../../../backend/services/device-validators/device-validators.service";
import GetValidatorsResponse from "../../../backend/services/device-validators/models/get-validators-response";
import { MessagesService } from "../../../backend/services/messages/messages.service";

@Component({
    selector: "app-add-business-form",
    templateUrl: "./add-business-form.component.html",
    styles: []
})
export class AddBusinessFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        name: {
            required: { message: "Name is required" }
        },
        logo: {
            required: { message: "Logo is required" },
            pattern: { message: "Logo must be a valid image URL" }
        },
        rut: {
            required: { message: "Rut is required" },
            pattern: {
                message:
                    "Rut must follow the correct format (e.g., 12.345.678-9)"
            }
        }
    };

    private _businessesSubscription: Subscription | null = null;
    private _validatorsSubscription: Subscription | null = null;
    businessForm!: FormGroup;
    status = { loading: false, error: null };

    validatorOptions: { label: string; value: string }[] = [];

    constructor(
        private _formBuilder: FormBuilder,
        private _router: Router,
        private _messagesService: MessagesService,
        private _businessesService: BusinessesService,
        private _validatorService: DeviceValidatorsService
    ) {}

    ngOnInit() {
        this.businessForm = this._formBuilder.group({
            name: ["", [Validators.required, Validators.maxLength(100)]],
            logo: [
                "",
                [
                    Validators.required,
                    Validators.pattern(/^(http|https):\/\/[^ "]+$/)
                ]
            ],
            rut: [
                "",
                [
                    Validators.required,
                    Validators.pattern(/^\d{1,2}\.\d{3}\.\d{3}-[\dKk]{1}$/)
                ]
            ],
            validator: ["", []]
        });

        this._validatorsSubscription = this._validatorService
            .getDeviceValidators()
            .subscribe({
                next: (response: GetValidatorsResponse) => {
                    this.validatorOptions = response.validators.map(
                        (validator) => ({
                            label: validator,
                            value: validator
                        })
                    );
                },
                error: (error) => {
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    onSubmit() {
        this.status.loading = true;
        this.status.error = null;
        console.log(this.businessForm.value);
        if (this.businessForm.valid) {
            this._businessesSubscription = this._businessesService
                .postBusiness(this.businessForm.value)
                .subscribe({
                    next: (response: CreateBusinessResponse) => {
                        this.status.loading = false;
                        this._messagesService.add({
                            severity: "success",
                            summary: "Business Added",
                            detail: `The business ${response.rut} has been added successfully.`
                        });
                        this._router.navigate(["/businesses"]);
                    },
                    error: (error: any) => {
                        this.status.loading = false;
                        this.status.error = error;
                        this._messagesService.add({
                            severity: "error",
                            summary: "Error",
                            detail: error.message
                        });
                    }
                });
        }
    }

    ngOnDestroy() {
        this._businessesSubscription?.unsubscribe();
        this._validatorsSubscription?.unsubscribe();
    }
}
