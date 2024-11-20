import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MenuModule } from "primeng/menu";
import { SidebarModule } from "primeng/sidebar";
import Route from "./models/route";

@Component({
    selector: "app-sidebar",
    standalone: true,
    imports: [SidebarModule, MenuModule],
    templateUrl: "./sidebar.component.html"
})
export class SidebarComponent {
    @Input() visible = false;
    @Output() visibleChange = new EventEmitter<boolean>();
    @Input() routes: Route[] = [];

    handleHide() {
        this.visible = false;
        this.visibleChange.emit(this.visible);
    }
}
