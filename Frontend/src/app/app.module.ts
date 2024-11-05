import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

import { AppRoutingModule } from "./app-routing.module";
import { AppComponent } from "./app.component";
import { ToolbarComponent } from "../components/toolbar/toolbar.component";
import { SidebarComponent } from "../components/sidebar/sidebar.component";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { provideHttpClient } from "@angular/common/http";
import { HomeComponent } from "./home/home.component";

@NgModule({
    declarations: [AppComponent, PageNotFoundComponent, HomeComponent],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        ToolbarComponent,
        SidebarComponent
    ],
    providers: [provideHttpClient()],
    bootstrap: [AppComponent]
})
export class AppModule {}
