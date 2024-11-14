import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BusinessesRoutingModule } from './businesses-routing.module';
import { BusinessesPageComponent } from './businesses-page/businesses-page.component';
import { PanelComponent } from "../../components/panel/panel.component";
import { BusinessesTableComponent } from "../../business-components/businesses-table/businesses-table.component";
import { ButtonComponent } from '../../components/button/button.component';


@NgModule({
  declarations: [
    BusinessesPageComponent
  ],
  imports: [
    CommonModule,
    BusinessesRoutingModule,
    PanelComponent,
    BusinessesTableComponent,
    ButtonComponent
]
})
export class BusinessesModule { }
