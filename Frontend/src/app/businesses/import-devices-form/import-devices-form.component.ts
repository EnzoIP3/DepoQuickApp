import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import GetImportFilesResponse from "../../../backend/services/importers/models/get-import-files-response";
import GetImportersResponse from "../../../backend/services/importers/models/get-importers-response";
import { DeviceImportFilesService } from "../../../backend/services/importers/device-import-files.service";
import { DeviceImportersService } from "../../../backend/services/importers/device-importers.service";
import { DevicesService } from "../../../backend/services/devices/devices.service";
import ImportDevicesRequest from "../../../backend/services/devices/models/import-devices-request";
import { Component, OnInit, OnDestroy, Input } from "@angular/core";

@Component({
    selector: "app-import-devices-form",
    templateUrl: "./import-devices-form.component.html",
    styles: []
})
export class ImportDevicesFormComponent implements OnInit, OnDestroy {
    @Input() businessId!: string;
    selectedImporterParameters: string[] = [];
    importDevicesForm: FormGroup;
    importerOptions: { label: string; value: string }[] = [];
    fileOptions: { label: string; value: string }[] = [];
    showConfirmationDialog = false;
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
        this.importDevicesForm = this.fb.group({
            importer: ["", Validators.required],
            importFile: ["", Validators.required]
        });
    }

    ngOnInit(): void {
        this.status.loading = true;

        this._importersSubscription = this._deviceImportersService
            .getImporters()
            .subscribe({
                next: (response: GetImportersResponse) => {
                    this.importerOptions = response.importers.map(
                        (importerName) => ({
                            label: importerName,
                            value: importerName
                        })
                    );
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
                route: this.importDevicesForm.value.importFile
            };
            this.status.loading = true;
            this._devicesSubscription = this._deviceService
                .importDevices(request)
                .subscribe({
                    next: () => {
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
