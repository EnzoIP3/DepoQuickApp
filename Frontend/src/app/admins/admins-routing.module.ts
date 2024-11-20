import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AddAdminFormComponent } from "./add-admin-form/add-admin-form.component";
import { AddBusinessOwnerFormComponent } from "./add-business-owner-form/add-business-owner-form.component";
import { AdminsPageComponent } from "./admins-page/admins-page.component";

const routes: Routes = [
    { path: "", component: AdminsPageComponent },
    { path: "new", component: AddAdminFormComponent },
    { path: "business_owner/new", component: AddBusinessOwnerFormComponent }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AdminsRoutingModule {}
