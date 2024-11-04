import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PageNotFoundComponent } from "./page-not-found/page-not-found.component";
import { noAuthGuard } from "../guards/no-auth.guard";
import { authGuard } from "../guards/auth.guard";
import { HomeComponent } from "./home/home.component";

const routes: Routes = [
    {
        path: "home",
        canActivate: [authGuard],
        component: HomeComponent
    },
    {
        path: "",
        canActivate: [noAuthGuard],
        loadChildren: () =>
            import("./auth/auth.module").then((m) => m.AuthModule)
    },
    {
        path: "**",
        component: PageNotFoundComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}
