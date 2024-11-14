import { Component, Input } from "@angular/core";
import {
    FormBuilder,
    FormGroup,
    ReactiveFormsModule,
    Validators
} from "@angular/forms";
import { MessageService } from "primeng/api";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { FormComponent } from "../../../components/form/form/form.component";
import { FormMultiSelectComponent } from "../../../components/form/form-multi-select/form-multi-select.component";
import { FormButtonComponent } from "../../../components/form/form-button/form-button.component";
import { DevicesService } from "../../../backend/services/devices/devices.service";
import Device from "../../../backend/services/devices/models/device";

@Component({
    standalone: true,
    selector: "app-add-device-form",
    imports: [
        FormComponent,
        FormMultiSelectComponent,
        FormButtonComponent,
        ReactiveFormsModule
    ],
    templateUrl: "./add-device-form.component.html"
})
export class AddDeviceFormComponent {
    @Input() homeId!: string;

    deviceForm!: FormGroup;
    devicesLoading = true;
    devicesFormLoading = false;
    availableDevices: Device[] = [];

    constructor(
        private _formBuilder: FormBuilder,
        private _homesService: HomesService,
        private _devicesService: DevicesService,
        private _messageService: MessageService
    ) {}

    ngOnInit() {
        this.deviceForm = this._formBuilder.group({
            devices: [[], [Validators.required]]
        });

        this._devicesService.getDevices().subscribe({
            next: (response) => {
                this.availableDevices = response.devices;
                this.devicesLoading = false;
            },
            error: (error) => {
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
            }
        });
    }

    onSubmit() {
        this.devicesFormLoading = true;
        this._homesService
            .addDevicesToHome(this.homeId, {
                deviceIds: this.deviceForm.value.devices
            })
            .subscribe({
                next: () => {
                    this.devicesFormLoading = false;
                    this.deviceForm.reset();
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Devices associated successfully with the home"
                    });
                },
                error: (error) => {
                    this.devicesFormLoading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }
}
