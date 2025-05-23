import { Component, Input, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";
import { DevicesService } from "../../../backend/services/devices/devices.service";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { MessagesService } from "../../../backend/services/messages/messages.service";

@Component({
    selector: "app-name-device-form",
    templateUrl: "./name-device-form.component.html"
})
export class NameDeviceFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        newName: {
            required: { message: "Name is required" }
        }
    };

    @Input() homeId!: string;
    @Input() hardwareId!: string;
    deviceForm!: FormGroup;
    deviceStatus = { loading: false };
    private _nameDeviceSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _devicesService: DevicesService,
        private _messagesService: MessagesService,
        private _homesService: HomesService
    ) {}

    ngOnInit() {
        this.deviceForm = this._formBuilder.group({
            newName: ["", [Validators.required]]
        });
    }

    onSubmit() {
        this.deviceStatus.loading = true;

        this._nameDeviceSubscription = this._devicesService
            .nameDevice(this.hardwareId, this.deviceForm.value)
            .subscribe({
                next: () => {
                    this.deviceStatus.loading = false;
                    this._messagesService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Device is now named ${this.deviceForm.value.newName}`
                    });
                    this.deviceForm.reset({
                        name: ""
                    });
                    this._homesService.getDevices(this.homeId).subscribe();
                },
                error: (error) => {
                    this.deviceStatus.loading = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._nameDeviceSubscription?.unsubscribe();
    }
}
