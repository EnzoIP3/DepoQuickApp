import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { CheckboxModule } from "primeng/checkbox";

@Component({
    selector: "app-form-checkbox",
    standalone: true,
    imports: [CommonModule, CheckboxModule, ReactiveFormsModule],
    templateUrl: "./form-checkbox.component.html"
})
export class FormCheckboxComponent {
    @Input() form!: FormGroup;
    @Input() name!: string;
    @Input() label!: string;

    get error(): boolean {
        const control = this.form.get(this.name)!;
        return Boolean(control && control.touched && control.errors);
    }

    get errorClass() {
        return {
            "ng-invalid": this.error,
            "ng-dirty": this.error
        };
    }
}
