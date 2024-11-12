import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomesRoutingModule } from "./homes-routing.module";
import { HomesPageComponent } from "./homes-page/homes-page.component";
import { PanelComponent } from "../../components/panel/panel.component";
import { HomesTableComponent } from "../../business-components/homes-table/homes-table.component";
import { HomePageComponent } from "./home-page/home-page.component";
import { HomeListComponent } from "../../business-components/home-list/home-list.component";
import { ButtonComponent } from "../../components/button/button.component";
import { AddHomeFormComponent } from "./add-home-form/add-home-form.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { IconTitleComponent } from "../../components/icon-title/icon-title.component";
import { FormComponent } from "../../components/form/form/form.component";

@NgModule({
    declarations: [HomesPageComponent, HomePageComponent, AddHomeFormComponent],
    imports: [
        CommonModule,
        HomesRoutingModule,
        PanelComponent,
        HomesTableComponent,
        HomeListComponent,
        ButtonComponent,
        FormButtonComponent,
        FormInputComponent,
        IconTitleComponent,
        FormComponent
    ]
})
export class HomesModule {}
