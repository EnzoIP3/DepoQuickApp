import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminsRoutingModule } from './admins-routing.module';
import { AdminsPageComponent } from './admins-page/admins-page.component';
import { UsersTableComponent } from "../../business-components/users-table/users-table.component";
import { BusinessesTableComponent } from "../../business-components/businesses-table/businesses-table.component";
import { AddAdminFormComponent } from './add-admin-form/add-admin-form.component';
import { AddBusinessOwnerFormComponent } from './add-businessowner-form/add-businessowner-form/add-businessowner-form.component';
import { FormComponent } from "../../components/form/form/form.component";
import { IconTitleComponent } from "../../components/icon-title/icon-title.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { PanelComponent } from "../../components/panel/panel.component";
import { ButtonComponent } from '../../components/button/button.component';
import { AddDeviceFormComponent } from "../homes/add-device-form/add-device-form.component";
import { DeleteAdminButtonComponent } from "../../business-components/delete-admin-button/delete-admin-button/delete-admin-button.component";


@NgModule({
  declarations: [
    AdminsPageComponent,
    AddAdminFormComponent,
    AddBusinessOwnerFormComponent
  ],
  imports: [
    CommonModule,
    AdminsRoutingModule,
    UsersTableComponent,
    BusinessesTableComponent,
    FormComponent,
    IconTitleComponent,
    FormInputComponent,
    FormButtonComponent,
    PanelComponent,
    ButtonComponent,
    AddDeviceFormComponent,
    DeleteAdminButtonComponent
]
})
export class AdminsModule { }
