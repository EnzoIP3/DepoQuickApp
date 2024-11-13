import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { authGuard } from "../../guards/auth.guard";
import { noAuthGuard } from "../../guards/no-auth.guard";
import { RootPageComponent } from "./root-page/root-page.component";

const routes: Routes = [
    {
        path: "",
        component: RootPageComponent,
        canActivate: [noAuthGuard],
        loadChildren: () =>
            import("../auth/auth.module").then((m) => m.AuthModule)
    },
    {
        path: "home",
        component: RootPageComponent,
        canActivate: [authGuard],
        children: [
            {
                path: "",
                loadChildren: () =>
                    import("../home/home.module").then((m) => m.HomeModule)
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RootRoutingModule {}
