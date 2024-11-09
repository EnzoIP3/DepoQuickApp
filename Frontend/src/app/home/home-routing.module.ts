import { Component, NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HomePageComponent } from "./home-page/home-page.component";
import { authGuard } from "../../guards/auth.guard";
import { noAuthGuard } from "../../guards/no-auth.guard";

const routes: Routes = [
    {
        path: "",
        component: HomePageComponent,
        canActivate: [noAuthGuard],
        loadChildren: () =>
            import("../auth/auth.module").then((m) => m.AuthModule)
    },
    {
        path: "home",
        component: HomePageComponent,
        canActivate: [authGuard],
        children: [
            {
                path: "",
                component: Component
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule {}
