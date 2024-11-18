import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminsRoutingModule } from './admins-routing.module';
import { AdminsPageComponent } from './admins-page/admins-page.component';
import { UsersTableComponent } from "../../business-components/users-table/users-table.component";


@NgModule({
  declarations: [
    AdminsPageComponent
  ],
  imports: [
    CommonModule,
    AdminsRoutingModule,
    UsersTableComponent
]
})
export class AdminsModule { }
