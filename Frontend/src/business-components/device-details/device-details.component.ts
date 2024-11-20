import {
    Component,
    Input,
    OnChanges,
    OnDestroy,
    SimpleChanges
} from "@angular/core";
import { Subscription } from "rxjs";
import { MessageService } from "primeng/api";
import Device from "../../backend/services/devices/models/device";
import { CommonModule } from "@angular/common";
import { CamerasService } from "../../backend/services/cameras/cameras.service";
import ListItem from "../../components/list/models/list-item";
import { ListComponent } from "../../components/list/list.component";
import { DividerComponent } from "../../components/divider/divider.component";

@Component({
    selector: "app-device-details",
    standalone: true,
    templateUrl: "./device-details.component.html",
    imports: [CommonModule, ListComponent, DividerComponent],
    providers: [MessageService]
})
export class DeviceDetailsComponent implements OnChanges, OnDestroy {
    @Input() device!: Device;
    @Input() imageWidth = "100%";

    extraCameraFields: {
        motionDetection?: boolean;
        personDetection?: boolean;
        isExterior?: boolean;
        isInterior?: boolean;
    } | null = null;

    private _camerasSubscription: Subscription | null = null;

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
        this._camerasSubscription?.unsubscribe();
    }

    private loadCameraDetails(deviceId: string): void {
        this._camerasSubscription = this.camerasService
            .getCameraDetails(deviceId)
            .subscribe({
                next: (cameraDetails) => {
                    this.extraCameraFields = {
                        motionDetection: cameraDetails.motionDetection,
                        personDetection: cameraDetails.personDetection,
                        isExterior: cameraDetails.exterior,
                        isInterior: cameraDetails.interior
                    };
                },
                error: (err) => {
                    this.extraCameraFields = null;
                    this.messageService.add({
                        severity: "error",
                        summary: "Error fetching camera details",
                        detail: err.message || "An unexpected error occurred."
                    });
                }
            });
    }

    private resetCameraDetails(): void {
        this.extraCameraFields = null;
    }

    get details(): ListItem[] {
        return [
            {
                label: "Id",
                value: this.device.id
            },
            {
                label: "Name",
                value: this.device.name
            },
            {
                label: "Business Name",
                value: this.device.businessName
            },
            {
                label: "Type",
                value: this.device.type
            },
            {
                label: "Model Number",
                value: this.device.modelNumber
            }
        ];
    }

    get cameraDetails(): ListItem[] {
        return [
            {
                label: "Motion Detection",
                value: this.extraCameraFields?.motionDetection ? "Yes" : "No"
            },
            {
                label: "Person Detection",
                value: this.extraCameraFields?.personDetection ? "Yes" : "No"
            },
            {
                label: "Is Exterior",
                value: this.extraCameraFields?.isExterior ? "Yes" : "No"
            },
            {
                label: "Is Interior",
                value: this.extraCameraFields?.isInterior ? "Yes" : "No"
            }
        ];
    }
}
