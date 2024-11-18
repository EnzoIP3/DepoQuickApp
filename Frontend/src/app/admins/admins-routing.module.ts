import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminsPageComponent } from './admins-page/admins-page.component';

const routes: Routes = [{
  path: "",
  component: AdminsPageComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminsRoutingModule { }
