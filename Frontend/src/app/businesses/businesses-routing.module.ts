import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessesPageComponent } from './businesses-page/businesses-page.component';
import { AddBusinessFormComponent } from './add-business-form/add-business-form.component';

const routes: Routes = [
  {
    path: "",
    component: BusinessesPageComponent
  },
  {
    path: "new",
    component: AddBusinessFormComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessesRoutingModule { }
