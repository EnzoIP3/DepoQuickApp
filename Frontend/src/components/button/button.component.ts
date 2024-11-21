import { Component, EventEmitter, Input, Output } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ButtonModule } from "primeng/button";

@Component({
    selector: "app-button",
    standalone: true,
    imports: [ButtonModule, RouterModule],
    templateUrl: "./button.component.html"
})
export class ButtonComponent {
    @Input() label = "";
    @Input() icon = "";
    @Input() routerLink: string | null = null;
    @Input() disabled = false;
    @Input() severity:
        | "success"
        | "info"
        | "warning"
        | "danger"
        | "help"
        | "primary"
        | "secondary"
        | "contrast"
        | null
        | undefined;
    @Input() className: string | null = null;
    @Output() click = new EventEmitter<any>();
}
