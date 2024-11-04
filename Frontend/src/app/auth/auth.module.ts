import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { AuthRoutingModule } from "./auth-routing.module";
import { AuthPageComponent } from "./auth-page/auth-page.component";
import { LoginFormComponent } from "./login-form/login-form.component";
import { RegisterFormComponent } from "./register-form/register-form.component";
import { FormComponent } from "../../components/form/form/form.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { MessageComponent } from "../../components/message/message.component";
import { IconTitleComponent } from "../../components/icon-title/icon-title.component";

@NgModule({
    declarations: [
        AuthPageComponent,
        LoginFormComponent,
        RegisterFormComponent
    ],
    imports: [
        CommonModule,
        AuthRoutingModule,
        FormComponent,
        FormInputComponent,
        FormButtonComponent,
        MessageComponent,
        IconTitleComponent
    ]
})
export class AuthModule {}
