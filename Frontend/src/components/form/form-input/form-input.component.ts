import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { IconFieldModule } from "primeng/iconfield";
import { InputIconModule } from "primeng/inputicon";
import { InputTextModule } from "primeng/inputtext";
import { FormField } from "./models/form-field";

@Component({
    selector: "app-form-input",
    standalone: true,
    imports: [
        CommonModule,
        IconFieldModule,
        InputIconModule,
        InputTextModule,
        ReactiveFormsModule
    ],
    templateUrl: "./form-input.component.html"
})
export class FormInputComponent {
    @Input() form!: FormGroup;
    @Input() formField!: FormField;
    @Input() name!: string;
    @Input() type = "text";
    @Input() label!: string;
    @Input() placeholder!: string;
    @Input() iconClass!: string;

    private hasValidationError(): boolean {
        const control = this.getFormControl();
        return control.errors! && control.touched;
    }

    private getFormControl() {
        return this.form.get(this.name)!;
    }

    private getErrorMessage(): string {
        const control = this.getFormControl();
        const errorKey = this.getFirstErrorKey(control);
        return this.formField[errorKey].message;
    }

    private getFirstErrorKey(control: any): string {
        return Object.keys(control.errors)[0];
    }

    get error(): string | null {
        return !this.hasValidationError() ? null : this.getErrorMessage();
    }

    get errorClass() {
        return {
            "ng-invalid": this.error,
            "ng-dirty": this.error
        };
    }
}
