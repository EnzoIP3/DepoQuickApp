import { Component, EventEmitter, Input, Output } from "@angular/core";
import { MenuItem } from "primeng/api";
import { MenuModule } from "primeng/menu";
import { SidebarModule } from "primeng/sidebar";

@Component({
    selector: "app-sidebar",
    standalone: true,
    imports: [SidebarModule, MenuModule],
    templateUrl: "./sidebar.component.html"
})
export class SidebarComponent {
    @Input() visible: boolean = false;
    @Output() visibleChange = new EventEmitter<boolean>();
    @Input() routes: MenuItem[] = [];

    handleHide() {
        this.visible = false;
        this.visibleChange.emit(this.visible);
    }
}
