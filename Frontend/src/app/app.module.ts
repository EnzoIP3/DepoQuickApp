import { importProvidersFrom, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { ToolbarComponent } from "../components/toolbar/toolbar.component";
import { SidebarComponent } from "../components/sidebar/sidebar.component";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { provideHttpClient } from "@angular/common/http";
import { PermissionSidebarComponent } from "../business-components/permission-sidebar/permission-sidebar.component";
import { MessageService } from "primeng/api";

@NgModule({
    declarations: [AppComponent, PageNotFoundComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        ToolbarComponent,
        SidebarComponent,
        PermissionSidebarComponent
    ],
    providers: [provideHttpClient(), MessageService],
    bootstrap: [AppComponent]
})
export class AppModule {}
