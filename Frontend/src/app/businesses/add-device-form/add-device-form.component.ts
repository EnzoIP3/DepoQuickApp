import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-device-form',
  templateUrl: './add-device-form.component.html',
  styles: []
})
export class AddDeviceFormComponent implements OnInit {
  deviceForm!: FormGroup;
  deviceTypes = [
    { label: 'Sensor', value: 'Sensor' },
    { label: 'Movement Sensor', value: 'Movement Sensor' },
    { label: 'Camera', value: 'Camera' },
    { label: 'Lamp', value: 'Lamp' }
  ];

  constructor(private fb: FormBuilder) {}

  ngOnInit() {
    this.deviceForm = this.fb.group({
      name: ['', Validators.required],
      modelNumber: ['', Validators.required],
      description: ['', Validators.required],
      mainPhoto: ['', Validators.required],
      secondaryPhotos: [''],
      type: ['', Validators.required],
      motionDetection: [null],
      personDetection: [null],
      isExterior: [false],
      isInterior: [true]
    });

    // Reactively show/hide Camera-specific fields
    this.deviceForm.get('type')?.valueChanges.subscribe((value) => {
      if (value === 'Camera') {
        this.deviceForm.get('motionDetection')?.setValidators(Validators.required);
        this.deviceForm.get('personDetection')?.setValidators(Validators.required);
      } else {
        this.deviceForm.get('motionDetection')?.clearValidators();
        this.deviceForm.get('personDetection')?.clearValidators();
      }
      this.deviceForm.get('motionDetection')?.updateValueAndValidity();
      this.deviceForm.get('personDetection')?.updateValueAndValidity();
    });
  }

  onSubmit() {
    if (this.deviceForm.valid) {
      console.log('Device Form Submitted:', this.deviceForm.value);
    } else {
      console.log('Form is invalid');
    }
  }
}
