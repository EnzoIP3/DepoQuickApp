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
    @Input() title: string = "";
    @Input() icon?: string;
    @Input() toggleable: boolean = true;
    @Input() collapsed: boolean = false;
}
