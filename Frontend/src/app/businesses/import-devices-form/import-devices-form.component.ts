import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import GetImportFilesResponse from '../../../backend/services/importers/models/get-import-files-response';
import GetImportersResponse from '../../../backend/services/importers/models/get-importers-response';
import { DeviceImportFilesService } from '../../../backend/services/importers/device-import-files.service';
import { DeviceImportersService } from '../../../backend/services/importers/device-importers.service';

@Component({
  selector: 'app-import-devices-form',
  templateUrl: './import-devices-form.component.html',
  styles: [],
})
export class ImportDevicesFormComponent implements OnInit, OnDestroy {
  importDevicesForm: FormGroup;

  importerOptions: { label: string; value: string }[] = [];
  fileOptions: { label: string; value: string }[] = [];

  private _importersSubscription: Subscription | null = null;
  private _filesSubscription: Subscription | null = null;

  status = { loading: false, error: null };

  constructor(
    private fb: FormBuilder,
    private _messageService: MessageService,
    private _deviceImportersService: DeviceImportersService,
    private _deviceImportFilesService: DeviceImportFilesService
  ) {
    // Definir el formulario
    this.importDevicesForm = this.fb.group({
      importer: ['', Validators.required],
      importFile: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.status.loading = true;

    this._importersSubscription = this._deviceImportersService.getImporters().subscribe({
      next: (response: GetImportersResponse) => {
        this.importerOptions = response.importers.map((importerName) => ({
          label: importerName,
          value: importerName,
        }));
        this.status.loading = false;
      },
      error: (error) => {
        this.status.loading = false;
        this.status.error = error;
        this._messageService.add({
          severity: 'error',
          summary: 'Error loading importers',
          detail: error.message,
        });
      },
    });

    this._filesSubscription = this._deviceImportFilesService.getImportFiles().subscribe({
      next: (response: GetImportFilesResponse) => {
        this.fileOptions = response.importFiles.map((file) => ({
          label: file,
          value: file,
        }));
      },
      error: (error) => {
        this.status.error = error;
        this._messageService.add({
          severity: 'error',
          summary: 'Error loading files',
          detail: error.message,
        });
      },
    });
  }

  onSubmit(): void {
    console.log(this.importDevicesForm.value);
  }

  ngOnDestroy(): void {
    this._importersSubscription?.unsubscribe();
    this._filesSubscription?.unsubscribe();
  }
}
