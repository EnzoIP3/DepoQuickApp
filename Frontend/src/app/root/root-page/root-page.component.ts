import { Component } from "@angular/core";

@Component({
    selector: "app-home-page",
    templateUrl: "./root-page.component.html"
})
export class RootPageComponent {
    title = "HomeConnect";
    sidebarVisible = false;

    openSidebar() {
        this.sidebarVisible = true;
    }
}
