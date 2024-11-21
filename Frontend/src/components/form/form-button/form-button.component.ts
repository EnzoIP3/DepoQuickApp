import { Component, Input } from "@angular/core";
import { ButtonModule } from "primeng/button";

@Component({
    selector: "app-form-button",
    standalone: true,
    imports: [ButtonModule],
    templateUrl: "./form-button.component.html"
})
export class FormButtonComponent {
    @Input() title!: string;
    @Input() loading = false;
    @Input() disabled = false;
}
