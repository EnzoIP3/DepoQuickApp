import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomesPageComponent } from "./homes-page/homes-page.component";
import { HomePageComponent } from "./home-page/home-page.component";

const routes: Routes = [
    {
        path: "",
        component: HomesPageComponent
    },
    {
        path: ":id",
        component: HomePageComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomesRoutingModule {}
