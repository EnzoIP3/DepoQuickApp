import { Component, Input } from "@angular/core";
import { ToastModule } from "primeng/toast";

@Component({
    selector: "app-toast",
    standalone: true,
    imports: [ToastModule],
    templateUrl: "./toast.component.html"
})
export class ToastComponent {
    @Input() position:
        | "top-left"
        | "top-center"
        | "top-right"
        | "bottom-left"
        | "bottom-center"
        | "bottom-right"
        | "center" = "bottom-right";

    @Input() life = 10000;
}
