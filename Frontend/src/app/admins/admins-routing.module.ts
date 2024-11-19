import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminsPageComponent } from './admins-page/admins-page.component';
import { AddAdminFormComponent } from './add-admin-form/add-admin-form.component';

const routes: Routes = [
  { path: '', component: AdminsPageComponent },
  { path: 'add-admin', component: AddAdminFormComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminsRoutingModule {}