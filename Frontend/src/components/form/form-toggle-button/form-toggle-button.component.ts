import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormGroup, ReactiveFormsModule } from "@angular/forms";
import { ToggleButtonModule } from "primeng/togglebutton";

@Component({
    selector: "app-form-toggle-button",
    standalone: true,
    imports: [CommonModule, ToggleButtonModule, ReactiveFormsModule],
    templateUrl: "./form-toggle-button.component.html"
})
export class FormToggleButtonComponent {
    @Input() form!: FormGroup;
    @Input() name!: string;
    @Input() onLabel!: string;
    @Input() offLabel!: string;
    @Input() onIcon!: string;
    @Input() offIcon!: string;
    @Output() onChange: EventEmitter<any> = new EventEmitter<any>();
}
