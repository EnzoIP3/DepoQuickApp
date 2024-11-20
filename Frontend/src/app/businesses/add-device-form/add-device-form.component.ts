import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { DeviceTypesService } from "../../../backend/services/device-types/device-types.service";
import DeviceTypesResponse from "../../../backend/services/device-types/models/device-types-response";
import { CamerasService } from "../../../backend/services/cameras/cameras.service";
import { LampsService } from "../../../backend/services/lamps/lamps.service";
import { SensorsService } from "../../../backend/services/sensors/sensors.service";
import { MotionSensorsService } from "../../../backend/services/movement-sensors/motion-sensors.service";

@Component({
    selector: "app-add-device-form",
    templateUrl: "./add-device-form.component.html",
    styles: []
})
export class AddDeviceFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        name: {
            required: { message: "Device name is required" }
        },
        modelNumber: {
            required: { message: "Model number is required" }
        },
        description: {
            required: { message: "Description is required" }
        },
        mainPhoto: {
            required: { message: "Main photo is required" },
            pattern: { message: "Main photo must be a valid image URL" }
        }
    };

    deviceForm!: FormGroup;
    status = { loading: false, error: null };
    deviceTypes: { label: string; value: string }[] = [];

    private _deviceSubscription: Subscription | null = null;
    private _deviceTypesSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _messageService: MessageService,
        private _cameraService: CamerasService,
        private _sensorService: SensorsService,
        private _lampService: LampsService,
        private _motionSensorService: MotionSensorsService,
        private _deviceTypesService: DeviceTypesService
    ) {}

    ngOnInit() {
        this._buildForm();
        this._getDeviceTypes();
        this._handleDeviceTypeChanges();
    }

    private _handleDeviceTypeChanges() {
        this.deviceForm.get("type")?.valueChanges.subscribe((value) => {
            if (value === "Camera") {
                this.deviceForm
                    .get("motionDetection")
                    ?.setValidators(Validators.required);
                this.deviceForm
                    .get("personDetection")
                    ?.setValidators(Validators.required);
            } else {
                this.deviceForm.get("motionDetection")?.clearValidators();
                this.deviceForm.get("personDetection")?.clearValidators();
            }
            this.deviceForm.get("motionDetection")?.updateValueAndValidity();
            this.deviceForm.get("personDetection")?.updateValueAndValidity();
        });
    }

    private _getDeviceTypes() {
        this._deviceTypesSubscription = this._deviceTypesService
            .getDeviceTypes()
            .subscribe({
                next: (response: DeviceTypesResponse) => {
                    this.deviceTypes = response.deviceTypes.map((type) => ({
                        label: type,
                        value: type
                    }));
                },
                error: (error) => {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: "Failed to load device types. " + error.message
                    });
                }
            });
    }

    private _buildForm() {
        this.deviceForm = this._formBuilder.group({
            name: ["", [Validators.required, Validators.maxLength(100)]],
            modelNumber: ["", [Validators.required, Validators.maxLength(50)]],
            description: ["", [Validators.required, Validators.maxLength(250)]],
            mainPhoto: [
                "",
                [
                    Validators.required,
                    Validators.pattern(/^(http|https):\/\/[^ "]+$/)
                ]
            ],
            secondaryPhotos: [],
            type: ["", [Validators.required]],
            motionDetection: [false, []],
            personDetection: [false, []],
            isExterior: [false, []],
            isInterior: [false, []]
        });
    }

    private _getServiceForType(type: string) {
        switch (type) {
            case "Camera":
                return this._cameraService;
            case "Sensor":
                return this._sensorService;
            case "Movement Sensor":
                return this._motionSensorService;
            case "Lamp":
                return this._lampService;
            default:
                throw new Error(`Unknown device type: ${type}`);
        }
    }

    onSubmit() {
        this.status.loading = true;
        this.status.error = null;

        if (this.deviceForm.valid) {
            const deviceType = this.deviceForm.get("type")?.value;
            const service = this._getServiceForType(deviceType);

            const formData = this._filterFormDataForDeviceType(deviceType);
            this._deviceSubscription = service.postDevice(formData).subscribe({
                next: (response) => {
                    this.status.loading = false;
                    this._messageService.add({
                        severity: "success",
                        summary: `${deviceType} Added`,
                        detail: `The ${deviceType.toLowerCase()} with ID: ${response.id} has been added successfully.`
                    });
                    this.deviceForm.reset();
                    this.deviceForm.patchValue({
                        motionDetection: false,
                        personDetection: false,
                        isExterior: false,
                        isInterior: false
                    });
                },
                error: (error: any) => {
                    this.status.loading = false;
                    this.status.error = error;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
        } else {
            this.status.loading = false;
            this._messageService.add({
                severity: "error",
                summary: "Form Error",
                detail: "Please correct the form errors and try again."
            });
        }
    }

    private _filterFormDataForDeviceType(type: string): any {
        const baseFields = {
            name: this.deviceForm.get("name")?.value,
            modelNumber: this.deviceForm.get("modelNumber")?.value,
            description: this.deviceForm.get("description")?.value,
            mainPhoto: this.deviceForm.get("mainPhoto")?.value,
            secondaryPhotos: this.deviceForm.get("secondaryPhotos")?.value
                ? this.deviceForm.get("secondaryPhotos")?.value.split(",")
                : []
        };

        switch (type) {
            case "Camera":
                return {
                    ...baseFields,
                    motionDetection:
                        this.deviceForm.get("motionDetection")?.value,
                    personDetection:
                        this.deviceForm.get("personDetection")?.value,
                    exterior: this.deviceForm.get("isExterior")?.value,
                    interior: this.deviceForm.get("isInterior")?.value
                };
            case "Sensor":
            case "Lamp":
            case "Movement Sensor":
            default:
                return baseFields;
        }
    }

    ngOnDestroy() {
        this._deviceSubscription?.unsubscribe();
        this._deviceTypesSubscription?.unsubscribe();
    }
}
