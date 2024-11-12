import { Component, Input } from "@angular/core";
import { RouterModule } from "@angular/router";
import { ButtonModule } from "primeng/button";

@Component({
    selector: "app-button",
    standalone: true,
    imports: [ButtonModule, RouterModule],
    templateUrl: "./button.component.html"
})
export class ButtonComponent {
    @Input() label: string = "";
    @Input() icon: string = "";
    @Input() routerLink: string | null = null;
    @Input() disabled: boolean = false;
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
}
