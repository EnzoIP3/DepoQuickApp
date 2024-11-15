import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-import-devices-form',
  templateUrl: './import-devices-form.component.html',
})

export class ImportDevicesFormComponent {
  importDevicesForm: FormGroup;

  // Opciones para los dropdowns
  importerOptions = [
    { label: 'JsonDeviceImporter', value: 'JsonDeviceImporter' }
  ];

  fileOptions = [
    { label: 'devices-to-import.json', value: 'devices-to-import.json' }
  ];

  constructor(private fb: FormBuilder) {
    // Definir el formulario
    this.importDevicesForm = this.fb.group({
      importer: ['JsonDeviceImporter', Validators.required],
      importFile: ['devices-to-import.json', Validators.required]
    });
  }

  // Método para manejar el envío del formulario
  onSubmit() {
    console.log(this.importDevicesForm.value);
  }
}
