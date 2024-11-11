import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomesRoutingModule } from './homes-routing.module';
import { HomesPageComponent } from './homes-page/homes-page.component';
import { PanelComponent } from "../../components/panel/panel.component";
import { HomesTableComponent } from "../../business-components/homes-table/homes-table.component";
import { HomePageComponent } from './home-page/home-page.component';
import { HomeTableComponent } from "../../business-components/home-table/home-table.component";


@NgModule({
  declarations: [
    HomesPageComponent,
    HomePageComponent
  ],
  imports: [
    CommonModule,
    HomesRoutingModule,
    PanelComponent,
    HomesTableComponent,
    HomeTableComponent
]
})
export class HomesModule { }
