import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomesRoutingModule } from "./homes-routing.module";
import { HomesPageComponent } from "./homes-page/homes-page.component";
import { PanelComponent } from "../../components/panel/panel.component";
import { HomesTableComponent } from "../../business-components/homes-table/homes-table.component";
import { HomePageComponent } from "./home-page/home-page.component";
import { HomeListComponent } from "../../business-components/home-list/home-list.component";
import { AddHomeFormComponent } from "./add-home-form/add-home-form.component";
import { FormButtonComponent } from "../../components/form/form-button/form-button.component";
import { FormInputComponent } from "../../components/form/form-input/form-input.component";
import { IconTitleComponent } from "../../components/icon-title/icon-title.component";
import { FormComponent } from "../../components/form/form/form.component";
import { AddMemberFormComponent } from "./add-member-form/add-member-form.component";
import { FormToggleButtonComponent } from "../../components/form/form-toggle-button/form-toggle-button.component";
import { MembersTableComponent } from "../../business-components/members-table/members-table.component";
import { AddDeviceFormComponent } from "./add-device-form/add-device-form.component";
import { HomeDevicesTableComponent } from "../../business-components/home-devices-table/home-devices-table.component";
import { NameHomeFormComponent } from "./name-home-form/name-home-form.component";
import { AddRoomFormComponent } from "./add-room-form/add-room-form.component";
import { ButtonComponent } from "../../components/button/button.component";
import { NameDeviceFormComponent } from "./name-device-form/name-device-form.component";
import { DividerComponent } from "../../components/divider/divider.component";

@NgModule({
    declarations: [
        HomesPageComponent,
        HomePageComponent,
        AddHomeFormComponent,
        AddMemberFormComponent,
        NameHomeFormComponent,
        AddRoomFormComponent,
        NameDeviceFormComponent
    ],
    imports: [
        CommonModule,
        HomesRoutingModule,
        PanelComponent,
        HomesTableComponent,
        HomeListComponent,
        FormButtonComponent,
        FormInputComponent,
        FormToggleButtonComponent,
        IconTitleComponent,
        FormComponent,
        MembersTableComponent,
        AddDeviceFormComponent,
        HomeDevicesTableComponent,
        ButtonComponent,
        DividerComponent
    ]
})
export class HomesModule {}
