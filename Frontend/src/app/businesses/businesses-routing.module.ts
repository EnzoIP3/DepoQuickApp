import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { BusinessesPageComponent } from "./businesses-page/businesses-page.component";
import { AddBusinessFormComponent } from "./add-business-form/add-business-form.component";
import { BusinessPageComponent } from "./business-page/business-page.component";
import { BusinessDevicesPageComponent } from "./business-devices-page/business-devices-page.component";

const routes: Routes = [
    {
        path: "",
        component: BusinessesPageComponent
    },
    {
        path: "new",
        component: AddBusinessFormComponent
    },
    {
        path: ":id",
        component: BusinessPageComponent
    },
    {
        path: ":id/devices",
        component: BusinessDevicesPageComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class BusinessesRoutingModule {}
