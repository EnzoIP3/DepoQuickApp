import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule } from "./home-routing.module";
import { HomePageComponent } from "./home-page/home-page.component";
import { DevicesTableComponent } from "../../business-components/devices-table/devices-table.component";
import { DeviceTypesTableComponent } from "../../business-components/device-types-table/device-types-table.component";
import { PanelComponent } from "../../components/panel/panel.component";

@NgModule({
    declarations: [HomePageComponent],
    imports: [
    CommonModule,
    HomeRoutingModule,
    DevicesTableComponent,
    DeviceTypesTableComponent,
    PanelComponent
]
})
export class HomeModule {}
