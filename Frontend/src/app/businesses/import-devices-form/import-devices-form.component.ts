import {
    ChangeDetectorRef,
    Component,
    Input,
    OnDestroy,
    OnInit
} from "@angular/core";
import {
    Form,
    FormBuilder,
    FormControl,
    FormGroup,
    Validators
} from "@angular/forms";
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
    @Input() businessId!: string;

    selectedImporterParameters: string[] = [];
    importDevicesForm: FormGroup;
    parameters: FormGroup;
    importerOptions: { label: string; value: string; parameters: string[] }[] =
        [];
    fileOptions: { label: string; value: string }[] = [];
    showConfirmationDialog: boolean = false;
    status = { loading: false, error: null };

    private _importersSubscription: Subscription | null = null;
    private _filesSubscription: Subscription | null = null;
    private _devicesSubscription: Subscription | null = null;

    constructor(
        private fb: FormBuilder,
        private _messageService: MessageService,
        private _deviceImportersService: DeviceImportersService,
        private _deviceImportFilesService: DeviceImportFilesService,
        private _deviceService: DevicesService,
        private cdr: ChangeDetectorRef
    ) {
        this.importDevicesForm = this.fb.group({
            importer: ["", Validators.required],
            parameters: this.fb.group({})
        });
        this.parameters = this.importDevicesForm.get("parameters") as FormGroup;
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
                parameters: this.importDevicesForm.value.parameters
            };
            console.log("request");
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

    onImporterChange(event: any): void {
        console.log("changed");
        console.log(event);
        const selectedImporter = this.importerOptions.find(
            (importer) => importer.value === event.value
        );
        console.log(selectedImporter);
        this.selectedImporterParameters = selectedImporter?.parameters || [];
        console.log(this.selectedImporterParameters);
        this._updateParameterFields();
    }

    private _updateParameterFields(): void {
        const parametersGroup = this.importDevicesForm.get(
            "parameters"
        ) as FormGroup;

        Object.keys(parametersGroup.controls).forEach((key) => {
            parametersGroup.removeControl(key);
        });

        this.selectedImporterParameters.forEach((param) => {
            console.log("voy a poner el parametro: " + param);
            parametersGroup.addControl(
                param.toString(),
                new FormControl("", Validators.required)
            );
        });
        console.log("parameters Group Controls:", parametersGroup.controls);
        console.log("keys parametersGroup");
        console.log(Object.keys(parametersGroup.controls));
    }

    closeDialog() {
        this.showConfirmationDialog = false;
    }

    onConfirm() {
        this.confirmImport(true);
        this.selectedImporterParameters = [];
        this._updateParameterFields();
        this.closeDialog();
    }
}
