import { Component } from "@angular/core";

@Component({
    selector: "app-root",
    template: `
        <app-sidebar [(visible)]="sidebarVisible"></app-sidebar>
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
