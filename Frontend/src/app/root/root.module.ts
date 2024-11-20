import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { PermissionSidebarComponent } from "../../business-components/permission-sidebar/permission-sidebar.component";
import { ToolbarComponent } from "../../components/toolbar/toolbar.component";
import { RootPageComponent } from "./root-page/root-page.component";
import { RootRoutingModule } from "./root-routing.module";
import { ToastComponent } from "../../components/toast/toast.component";
import { AddHomeOwnerRoleButtonComponent } from "../../business-components/add-home-owner-role-button/add-home-owner-role-button.component";
import { RoleDropdownComponent } from "../../business-components/role-dropdown/role-dropdown.component";

@NgModule({
    declarations: [RootPageComponent],
    imports: [
        CommonModule,
        RootRoutingModule,
        PermissionSidebarComponent,
        ToolbarComponent,
        ToastComponent,
        AddHomeOwnerRoleButtonComponent,
        RoleDropdownComponent
    ]
})
export class RootModule {}
