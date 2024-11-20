import { Component, OnInit, OnDestroy } from "@angular/core";
import {
    FormBuilder,
    FormGroup,
    Validators,
    AbstractControl
} from "@angular/forms";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { DeviceValidatorsService } from "../../../backend/services/device-validators/device-validators.service";
import { BusinessesService } from "../../../backend/services/businesses/businesses.service";
import GetValidatorsResponse from "../../../backend/services/device-validators/models/get-validators-response";
import UpdateValidatorRequest from "../../../backend/services/businesses/models/update-validator-request";
import UpdateValidatorResponse from "../../../backend/services/businesses/models/update-validator-request";

@Component({
    selector: "app-change-validator-form",
    templateUrl: "./change-validator-form.component.html",
    styles: []
})
export class ChangeValidatorFormComponent implements OnInit, OnDestroy {
    changeValidatorForm: FormGroup;
    validatorOptions: { label: string; value: string }[] = [
        { label: "No Validator", value: "" }
    ];

    private _validatorsSubscription: Subscription | null = null;
    private _updateValidatorSubscription: Subscription | null = null;
    private _businessId: string | null = null;

    status = { loading: false, error: null };

    constructor(
        private fb: FormBuilder,
        private _messageService: MessageService,
        private _deviceValidatorsService: DeviceValidatorsService,
        private _businessesService: BusinessesService
    ) {
        this.changeValidatorForm = this.fb.group({
            validator: ["", this.validatorRequired]
        });
    }

    ngOnInit(): void {
        this._setBusinessId();
        this.status.loading = true;

        this._validatorsSubscription = this._deviceValidatorsService
            .getDeviceValidators()
            .subscribe({
                next: (response: GetValidatorsResponse) => {
                    this.validatorOptions = [
                        { label: "No Validator", value: "" },
                        ...response.validators.map((validator) => ({
                            label: validator,
                            value: validator
                        }))
                    ];
                    this.status.loading = false;
                },
                error: (error) => {
                    this.status.loading = false;
                    this.status.error = error;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error loading validators",
                        detail: error.message
                    });
                }
            });
    }

    validatorRequired(control: AbstractControl) {
        if (control.value === "") {
            return null;
        }
        return Validators.required(control);
    }

    onSubmit(): void {
        this.status.loading = true;
        this.status.error = null;

        if (this.changeValidatorForm.valid && this._businessId) {
            const request: UpdateValidatorRequest = {
                validator: this.changeValidatorForm.value.validator
            };

            this._updateValidatorSubscription = this._businessesService
                .updateValidator(this._businessId, request)
                .subscribe({
                    next: (response: UpdateValidatorResponse) => {
                        this.status.loading = false;
                        const selectedValidator = this.validatorOptions.find(
                            (option) =>
                                option.value ===
                                this.changeValidatorForm.value.validator
                        );
                        this._messageService.add({
                            severity: "success",
                            summary: "Validator Updated",
                            detail:
                                "The validator has been successfully updated to " +
                                (selectedValidator
                                    ? selectedValidator.label
                                    : "Unknown")
                        });
                        this.changeValidatorForm.reset();
                    },
                    error: (error) => {
                        this.status.loading = false;
                        this.status.error = error;
                        this._messageService.add({
                            severity: "error",
                            summary: "Error updating validator",
                            detail: error.message
                        });
                    }
                });
        } else {
            this.status.loading = false;
            this._messageService.add({
                severity: "warn",
                summary: "Invalid Form",
                detail: "Please select a valid validator option."
            });
        }
    }

    private _setBusinessId(): void {
        const url = window.location.href;
        const urlSegments = url.split("/");
        this._businessId = urlSegments[urlSegments.length - 1];
    }

    ngOnDestroy(): void {
        this._validatorsSubscription?.unsubscribe();
        this._updateValidatorSubscription?.unsubscribe();
    }
}
