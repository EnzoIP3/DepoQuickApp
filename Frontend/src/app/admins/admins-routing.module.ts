import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminsPageComponent } from './admins-page/admins-page.component';
import { AddAdminFormComponent } from './add-admin-form/add-admin-form.component';
import { AddBusinessOwnerFormComponent } from './add-businessowner-form/add-businessowner-form/add-businessowner-form.component';

const routes: Routes = [
  { path: '', component: AdminsPageComponent },
  { path: 'add-admin', component: AddAdminFormComponent },
  { path: 'add-businessowner', component: AddBusinessOwnerFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminsRoutingModule {}