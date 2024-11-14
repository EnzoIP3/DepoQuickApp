import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-add-business-form',
  templateUrl: './add-business-form.component.html',
  styles: []
})
export class AddBusinessFormComponent {
  readonly formFields = {
    name: {
      required: { message: "Name is required" },
    },
    logo: {
      required: { message: "Logo is required" },
      pattern: { message: "Logo must be a valid image URL" },
    },
    rut: {
      required: { message: "Rut is required" },
      pattern: { message: "Rut must follow the correct format (e.g., 12.345.678-9)" },
    },
    validator: {
      required: { message: "Validator is required" },
    }
  };

  businessForm!: FormGroup;
  status = { loading: false, error: null };
  private _addHomeSubscription: Subscription | null = null;
  
  // Dropdown options for the validator field
  validatorOptions = [
    { label: 'SixCharacters3Letters3NumbersModelValidator', value: 'SixCharacters3Letters3NumbersModelValidator' },
    { label: 'SixLettersModelValidator', value: 'SixLettersModelValidator' },
  ];

  constructor(
    private _formBuilder: FormBuilder,
    private _router: Router,
    private _messageService: MessageService
  ) {}

  ngOnInit() {
    this.businessForm = this._formBuilder.group({
      name: ["", [Validators.required]],
      logo: ["", [Validators.required, Validators.pattern(/^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|bmp))$/)]], // Logo should be a valid image URL
      rut: ["", [Validators.required, Validators.pattern(/^\d{1,2}\.\d{3}\.\d{3}-[\dKk]{1}$/)]], // Example Rut pattern
      validator: ["", [Validators.required]],
      address: ["", [Validators.required, Validators.pattern(/^[A-Za-z\s]+ \d+$/)]],
      latitude: ["", [Validators.required, Validators.min(-90), Validators.max(90)]],
      longitude: ["", [Validators.required, Validators.min(-90), Validators.max(90)]],
      maxMembers: ["", [Validators.required, Validators.min(1)]]
    });
  }

  onSubmit() {
    this.status.loading = true;
    this.status.error = null;
    // Logic to submit the form goes here
    if (this.businessForm.valid) {
      // Submit data to the backend or do something with the data
    } else {
      this.status.loading = false;
      this._messageService.add({
        severity: "error",
        summary: "Form Error",
        detail: "Please fill in all the required fields correctly."
      });
    }
  }

  ngOnDestroy() {
    // Clean up if necessary
    this._addHomeSubscription?.unsubscribe();
  }
}
