import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ToggleButtonModule } from "primeng/togglebutton";

@Component({
    selector: "app-form-toggle-button",
    standalone: true,
    imports: [CommonModule, ToggleButtonModule, ReactiveFormsModule],
    templateUrl: "./form-toggle-button.component.html",
    styles: ``
})
export class FormToggleButtonComponent {
    @Input() form!: FormGroup;
    @Input() name!: string;
    @Input() onLabel!: string;
    @Input() offLabel!: string;
    @Input() onIcon!: string;
    @Input() offIcon!: string;
}
