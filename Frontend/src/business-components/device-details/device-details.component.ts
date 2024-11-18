import { Component, Input, OnChanges, OnDestroy, SimpleChanges } from "@angular/core";
import { Subscription } from "rxjs";
import { MessageService } from "primeng/api";
import Device from "../../backend/services/devices/models/device";
import { CommonModule } from "@angular/common";
import { CamerasService } from "../../backend/services/cameras/cameras.service";

@Component({
  selector: "app-device-details",
  standalone: true,
  templateUrl: "./device-details.component.html",
  imports: [CommonModule],
  providers: [MessageService],
})
export class DeviceDetailsComponent implements OnChanges, OnDestroy {
  @Input() device!: Device;
  @Input() imageWidth: string = "100%";

  cameraExtraDetails: {
    motionDetection?: boolean;
    personDetection?: boolean;
    isExterior?: boolean;
    isInterior?: boolean;
  } | null = null;

  private subscription: Subscription | null = null;

  constructor(
    private camerasService: CamerasService,
    private messageService: MessageService
  ) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes["device"] && this.device && this.device.type === "Camera") {
      this.loadCameraDetails(this.device.id);
    } else {
      this.resetCameraDetails();
    }
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }

  private loadCameraDetails(deviceId: string): void {

    this.subscription = this.camerasService.getCameraDetails(deviceId).subscribe({
      next: (cameraDetails) => {
        this.cameraExtraDetails = {
          motionDetection: cameraDetails.motionDetection,
          personDetection: cameraDetails.personDetection,
          isExterior: cameraDetails.exterior,
          isInterior: cameraDetails.interior,
        };
      },
      error: (err) => {
        this.cameraExtraDetails = null;
        this.messageService.add({
          severity: "error",
          summary: "Error fetching camera details",
          detail: err.message || "An unexpected error occurred.",
        });
      },
    });
  }

  private resetCameraDetails(): void {
    this.cameraExtraDetails = null;
  }
}
