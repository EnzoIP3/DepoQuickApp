import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BusinessesService } from '../../../backend/services/businesses/businesses.service';
import CreateBusinessResponse from '../../../backend/services/businesses/models/create-business-response';

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

  private _businessesSubscription: Subscription | null = null;
  businessForm!: FormGroup;
  status = { loading: false, error: null };
  
  // Dropdown options for the validator field
  validatorOptions = [
    { label: 'SixCharacters3Letters3NumbersModelValidator', value: 'SixCharacters3Letters3NumbersModelValidator' },
    { label: 'SixLettersModelValidator', value: 'SixLettersModelValidator' },
  ];

  constructor(
    private _formBuilder: FormBuilder,
    private _router: Router,
    private _messageService: MessageService,
    private _businessesService: BusinessesService
  ) {}

  ngOnInit() {
    this.businessForm = this._formBuilder.group({
      name: ["", [Validators.required]],
      logo: ["", [Validators.required, Validators.pattern(/^(https?:\/\/.*\.(?:png|jpg|jpeg|gif|bmp))$/)]], // Logo should be a valid image URL
      rut: ["", [Validators.required, Validators.pattern(/^\d{1,2}\.\d{3}\.\d{3}-[\dKk]{1}$/)]], // Example Rut pattern
      validator: ["", [Validators.required]]
    });
  }

    onSubmit() {
      this.status.loading = true;
      this.status.error = null;
      console.log(this.businessForm.value);
      if (this.businessForm.valid) {
        this._businessesSubscription = this._businessesService.postBusiness(this.businessForm.value).subscribe({
          next: (response: CreateBusinessResponse) => {
            this.status.loading = false;
            this._messageService.add({
              severity: "success",
              summary: "Business Added",
              detail: `The business ${response.rut} has been added successfully.`
            });
            this._router.navigate(["/businesses"]);
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
      }
    }

  ngOnDestroy() {
    // Clean up if necessary
    this._businessesSubscription?.unsubscribe();
  }
}