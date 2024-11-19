import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import GetImportFilesResponse from "../../../backend/services/importers/models/get-import-files-response";
import { DeviceImportFilesService } from "../../../backend/services/importers/device-import-files.service";
import { DeviceImportersService } from "../../../backend/services/importers/device-importers.service";
import { DevicesService } from "../../../backend/services/devices/devices.service";
import ImportDevicesResponse from "../../../backend/services/devices/models/import-devices-response";
import ImportDevicesRequest from "../../../backend/services/devices/models/import-devices-request";
import { GetImportersResponse } from "../../../backend/services/importers/models/get-importers-response";

@Component({
    selector: "app-import-devices-form",
    templateUrl: "./import-devices-form.component.html",
    styles: []
})
export class ImportDevicesFormComponent implements OnInit, OnDestroy {
    importDevicesForm: FormGroup;
    importerOptions: { label: string; value: string; parameters: string[] }[] = [];
    fileOptions: { label: string; value: string }[] = [];
    showConfirmationDialog: boolean = false;
    private _businessId: string = "123";
    private _importersSubscription: Subscription | null = null;
    private _filesSubscription: Subscription | null = null;
    private _devicesSubscription: Subscription | null = null;
    status = { loading: false, error: null };

    constructor(
        private fb: FormBuilder,
        private _messageService: MessageService,
        private _deviceImportersService: DeviceImportersService,
        private _deviceImportFilesService: DeviceImportFilesService,
        private _deviceService: DevicesService
    ) {
        this._setBusinessId();
        this.importDevicesForm = this.fb.group({
            importer: ["", Validators.required],
            parameters: [""],
        });
    }

    private _setBusinessId(): void {
        const url = window.location.href;
        const urlSegments = url.split("/");
        this._businessId = urlSegments[urlSegments.length - 1];
    }

    ngOnInit(): void {
        this.status.loading = true;

        this._importersSubscription = this._deviceImportersService
            .getImporters()
            .subscribe({
                next: (response: GetImportersResponse) => {
                    this.importerOptions = response.importers.map(
                        (importer) => ({
                            label: importer.name,
                            value: importer.name,
                            parameters: importer.parameters
                        })
                    );
                    console.log(response);
                    this.status.loading = false;
                },
                error: (error) => {
                    this.status.loading = false;
                    this.status.error = error;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error loading importers",
                        detail: error.message
                    });
                }
            });

        this._filesSubscription = this._deviceImportFilesService
            .getImportFiles()
            .subscribe({
                next: (response: GetImportFilesResponse) => {
                    this.fileOptions = response.importFiles.map((file) => ({
                        label: file,
                        value: file
                    }));
                },
                error: (error) => {
                    this.status.error = error;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error loading files",
                        detail: error.message
                    });
                }
            });
    }

    onSubmit(): void {
        console.log(this.importDevicesForm.value);
        if (this.importDevicesForm.valid) {
            this.showConfirmationDialog = true;
        } else {
            this._messageService.add({
                severity: "warn",
                summary: "Invalid Form",
                detail: "Please check the form for errors."
            });
        }
    }

    confirmImport(confirm: boolean): void {
        this.showConfirmationDialog = false;
        if (confirm) {
            const request: ImportDevicesRequest = {
                importerName: this.importDevicesForm.value.importer,
                parameters: this.importDevicesForm.value.importer.parameters,
            };
            console.log(request);
            this.status.loading = true;
            this._devicesSubscription = this._deviceService
                .importDevices(request)
                .subscribe({
                    next: (response: ImportDevicesResponse) => {
                        this.status.loading = false;
                        this._messageService.add({
                            severity: "success",
                            summary: "Devices Imported",
                            detail: "The devices have been successfully imported."
                        });
                        this.importDevicesForm.reset();
                    },
                    error: (error) => {
                        this.status.loading = false;
                        this.status.error = error;
                        this._messageService.add({
                            severity: "error",
                            summary: "Error importing devices",
                            detail: error.message
                        });
                    }
                });
        }
    }

    ngOnDestroy(): void {
        this._importersSubscription?.unsubscribe();
        this._filesSubscription?.unsubscribe();
        this._devicesSubscription?.unsubscribe();
    }

    closeDialog() {
        this.showConfirmationDialog = false;
    }

    onConfirm() {
        this.confirmImport(true);
        this.closeDialog();
    }
}
