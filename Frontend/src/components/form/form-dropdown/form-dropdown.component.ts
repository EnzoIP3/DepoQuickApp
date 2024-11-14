import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { DropdownModule } from "primeng/dropdown";

@Component({
    selector: "app-form-dropdown",
    standalone: true,
    imports: [CommonModule, DropdownModule, ReactiveFormsModule],
    templateUrl: "./form-dropdown.component.html"
})
export class FormDropdownComponent {
    @Input() form!: FormGroup;
    @Input() name!: string;
    @Input() label!: string;
    @Input() placeholder!: string;
    @Input() options: { label: string; value: any }[] = [];

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
