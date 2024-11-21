import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { authGuard } from "../../guards/auth.guard";
import { noAuthGuard } from "../../guards/no-auth.guard";
import { RootPageComponent } from "./root-page/root-page.component";
import { Permissions } from "../../backend/services/routes/constants/permissions";
import { permissionsGuard } from "../../guards/permissions.guard";

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
        canActivate: [authGuard, permissionsGuard],
        data: { permissions: [Permissions.CREATE_HOME] },
        loadChildren: () =>
            import("../homes/homes.module").then((m) => m.HomesModule)
    },
    {
        path: "businesses",
        component: RootPageComponent,
        canActivate: [authGuard, permissionsGuard],
        data: { permissions: [Permissions.CREATE_BUSINESS] },
        loadChildren: () =>
            import("../businesses/businesses.module").then(
                (m) => m.BusinessesModule
            )
    },
    {
        path: "notifications",
        component: RootPageComponent,
        canActivate: [authGuard, permissionsGuard],
        data: { permissions: [Permissions.GET_NOTIFICATIONS] },
        loadChildren: () =>
            import("../notifications/notifications.module").then(
                (m) => m.NotificationsModule
            )
    },
    {
        path: "admins",
        component: RootPageComponent,
        canActivate: [authGuard],
        loadChildren: () =>
            import("../admins/admins.module").then((m) => m.AdminsModule)
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class RootRoutingModule {}
