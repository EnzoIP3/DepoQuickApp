import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PermissionSidebarComponent } from "../../business-components/permission-sidebar/permission-sidebar.component";
import { ToolbarComponent } from "../../components/toolbar/toolbar.component";
import { RootPageComponent } from "./root-page/root-page.component";
import { RootRoutingModule } from "./root-routing.module";
import { ToastComponent } from "../../components/toast/toast.component";

@NgModule({
    declarations: [RootPageComponent],
    imports: [
        CommonModule,
        RootRoutingModule,
        PermissionSidebarComponent,
        ToolbarComponent,
        ToastComponent
    ]
})
export class RootModule {}
