import { Component, Input, Output, EventEmitter } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { IconFieldModule } from "primeng/iconfield";
import { InputIconModule } from "primeng/inputicon";
import { InputTextModule } from "primeng/inputtext";
import { ButtonModule } from "primeng/button";

@Component({
    selector: "app-form",
    standalone: true,
    imports: [
        IconFieldModule,
        InputIconModule,
        InputTextModule,
        ButtonModule,
        ReactiveFormsModule
    ],
    templateUrl: "./form.component.html"
})
export class FormComponent {
    @Input() form!: FormGroup;
    @Output() onSubmit = new EventEmitter<void>();

    handleSubmit() {
        this.form.markAllAsTouched();
        if (this.form.valid) {
            this.onSubmit.emit(this.form.value);
        }
    }
}
