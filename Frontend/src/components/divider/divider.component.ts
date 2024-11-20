import { Component } from "@angular/core";
import { DividerModule } from "primeng/divider";

@Component({
    selector: "app-divider",
    standalone: true,
    imports: [DividerModule],
    templateUrl: "./divider.component.html"
})
export class DividerComponent {}
