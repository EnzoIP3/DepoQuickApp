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
        path: "devices",
        component: RootPageComponent,
        canActivate: [authGuard],
        loadChildren: () =>
            import("../devices/devices.module").then((m) => m.DevicesModule)
    },
    {
        path: "homes",
        component: RootPageComponent,
        canActivate: [authGuard],
        loadChildren: () =>
            import("../homes/homes.module").then((m) => m.HomesModule)
    },
    {
        path: "businesses",
        component: RootPageComponent,
        canActivate: [authGuard],
        loadChildren: () =>
            import("../businesses/businesses.module").then(
                (m) => m.BusinessesModule
            )
    },
    {
        path: "notifications",
        component: RootPageComponent,
        canActivate: [authGuard],
        loadChildren: () =>
            import("../notifications/notifications.module").then(
                (m) => m.NotificationsModule
            )
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RootRoutingModule {}
