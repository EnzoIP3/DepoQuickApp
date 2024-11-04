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

    items: MenuItem[] = [
        {
            label: "Login",
            icon: "pi pi-sign-in",
            routerLink: ["/login"],
            routerLinkActiveOptions: { exact: true }
        },
        {
            label: "Register",
            icon: "pi pi-user-plus",
            routerLink: ["/register"]
        }
    ];

    handleHide() {
        this.visible = false;
        this.visibleChange.emit(this.visible);
    }
}
