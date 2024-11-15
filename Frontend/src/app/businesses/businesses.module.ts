import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BusinessesRoutingModule } from './businesses-routing.module';
import { BusinessesPageComponent } from './businesses-page/businesses-page.component';
import { PanelComponent } from "../../components/panel/panel.component";
import { BusinessesTableComponent } from "../../business-components/businesses-table/businesses-table.component";
import { ButtonComponent } from '../../components/button/button.component';
import { AddBusinessFormComponent } from './add-business-form/add-business-form.component';
import { IconTitleComponent } from '../../components/icon-title/icon-title.component';
import { FormComponent } from '../../components/form/form/form.component';
import { FormInputComponent } from '../../components/form/form-input/form-input.component';
import { FormButtonComponent } from '../../components/form/form-button/form-button.component';
import { FormDropdownComponent } from '../../components/form/form-dropdown/form-dropdown.component';
import { BusinessPageComponent } from './business-page/business-page.component';
import { AddDeviceFormComponent } from './add-device-form/add-device-form.component';
import { FormCheckboxComponent } from '../../components/form/form-checkbox/form-checkbox.component';


@NgModule({
  declarations: [
    BusinessesPageComponent,
    AddBusinessFormComponent,
    BusinessPageComponent,
    AddDeviceFormComponent
  ],
  imports: [
    CommonModule,
    BusinessesRoutingModule,
    PanelComponent,
    BusinessesTableComponent,
    ButtonComponent,
    IconTitleComponent,
    FormComponent,
    FormInputComponent,
    FormButtonComponent,
    FormDropdownComponent,
    FormCheckboxComponent
]
})
export class BusinessesModule { }
