import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { HomeRoutingModule as DevicesRoutingModule } from "./devices-routing.module";
import { DevicesPageComponent } from "./devices-page/devices-page.component";
import { DevicesTableComponent } from "../../business-components/devices-table/devices-table.component";
import { DeviceTypesTableComponent } from "../../business-components/device-types-table/device-types-table.component";
import { PanelComponent } from "../../components/panel/panel.component";

@NgModule({
    declarations: [DevicesPageComponent],
    imports: [
        CommonModule,
        DevicesRoutingModule,
        DevicesTableComponent,
        DeviceTypesTableComponent,
        PanelComponent
    ]
})
export class HomeModule {}
