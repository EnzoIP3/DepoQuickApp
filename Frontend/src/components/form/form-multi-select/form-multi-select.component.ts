import { Component, Input } from "@angular/core";
import { MultiSelectModule } from "primeng/multiselect";
import { FormGroup, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { CommonModule } from "@angular/common";

@Component({
    selector: "app-form-multi-select",
    standalone: true,
    imports: [
        MultiSelectModule,
        FormsModule,
        CommonModule,
        ReactiveFormsModule
    ],
    templateUrl: "./form-multi-select.component.html"
})
export class FormMultiSelectComponent {
    @Input() form!: FormGroup;
    @Input() optionLabel = "name";
    @Input() optionValue = "id";
    @Input() name!: string;
    @Input() values: any[] = [];
    @Input() placeholder = "Select Options";
    @Input() loading = false;
}
