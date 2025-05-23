import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { PanelModule } from "primeng/panel";

@Component({
    selector: "app-panel",
    standalone: true,
    imports: [CommonModule, PanelModule],
    templateUrl: "./panel.component.html"
})
export class PanelComponent {
    @Input() title = "";
    @Input() icon?: string;
    @Input() toggleable = true;
    @Input() collapsed = false;
}
