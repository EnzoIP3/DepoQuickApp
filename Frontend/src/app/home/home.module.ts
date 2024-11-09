import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomePageComponent } from "./home-page/home-page.component";
import { PermissionSidebarComponent } from "../../business-components/permission-sidebar/permission-sidebar.component";
import { ToolbarComponent } from "../../components/toolbar/toolbar.component";
import { PlaceholderHomeComponent } from "./placeholder-home/placeholder-home.component";
import { TableComponent } from "../../components/table/table.component";

@NgModule({
    declarations: [HomePageComponent, PlaceholderHomeComponent],
    imports: [
        CommonModule,
        HomeRoutingModule,
        PermissionSidebarComponent,
        ToolbarComponent,
        TableComponent
    ]
})
export class HomeModule {}
