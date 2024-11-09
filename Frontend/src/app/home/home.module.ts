import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomePageComponent } from "./home-page/home-page.component";
import { PermissionSidebarComponent } from "../../business-components/permission-sidebar/permission-sidebar.component";
import { ToolbarComponent } from "../../components/toolbar/toolbar.component";
import { TableComponent } from "../../components/table/table.component";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { IconTitleComponent } from "../../components/icon-title/icon-title.component";
import { PanelComponent } from "../../components/panel/panel.component";

@NgModule({
    declarations: [HomePageComponent],
    imports: [
    CommonModule,
    HomeRoutingModule,
    PermissionSidebarComponent,
    ToolbarComponent,
    TableComponent,
    PaginatorComponent,
    IconTitleComponent,
    PanelComponent
]
})
export class HomeModule {}
