import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddAdminFormComponent } from "./add-admin-form/add-admin-form.component";
import { AddBusinessOwnerFormComponent } from "./add-business-owner-form/add-business-owner-form.component";
import { AdminsPageComponent } from "./admins-page/admins-page.component";
import { permissionsGuard } from "../../guards/permissions.guard";
import { Permissions } from "../../backend/services/routes/constants/permissions";

const routes: Routes = [
    {
        path: "",
        component: AdminsPageComponent,
        canActivate: [permissionsGuard],
        data: {
            permissions: [
                Permissions.GET_ALL_USERS,
                Permissions.GET_ALL_BUSINESSES
            ]
        }
    },
    {
        path: "new",
        component: AddAdminFormComponent,
        canActivate: [permissionsGuard],
        data: { permissions: [Permissions.CREATE_ADMINISTATOR] }
    },
    {
        path: "business_owner/new",
        component: AddBusinessOwnerFormComponent,
        canActivate: [permissionsGuard],
        data: { permissions: [Permissions.CREATE_BUSINESS_OWNER] }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminsRoutingModule {}
