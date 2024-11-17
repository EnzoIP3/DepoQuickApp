import { Component, Input } from "@angular/core";
import Device from "../../backend/services/devices/models/device";
import { CommonModule } from "@angular/common";

@Component({
  selector: "app-device-details",
  standalone: true,
  templateUrl: "./device-details.component.html",
  imports: [CommonModule],
})
export class DeviceDetailsComponent {
  @Input() device!: Device;
  @Input() imageWidth: string = "100%";
}
