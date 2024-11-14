import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BusinessesPageComponent } from './businesses-page/businesses-page.component';

const routes: Routes = [
  {
    path: "",
    component: BusinessesPageComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BusinessesRoutingModule { }
