import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { AuthPageComponent } from "./auth-page/auth-page.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { RegisterFormComponent } from "./register-form/register-form.component";

const routes: Routes = [
    {
        path: "",
        component: AuthPageComponent,
        children: [
            {
                path: "login",
                component: LoginFormComponent
            },
            {
                path: "register",
                component: RegisterFormComponent
            },
            {
                path: "",
                redirectTo: "login",
                pathMatch: "full"
            }
        ]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class AuthRoutingModule {}
