import { Component } from "@angular/core";

@Component({
    selector: "app-root",
    template: `
        <app-permission-sidebar
            [(sidebarVisible)]="sidebarVisible"
        ></app-permission-sidebar>
        <app-toolbar [title]="title" (onClick)="openSidebar()"></app-toolbar>
        <router-outlet />
    `,
    styles: []
})
export class AppComponent {
    title = "HomeConnect";
    sidebarVisible = false;

    openSidebar() {
        this.sidebarVisible = true;
    }
}
