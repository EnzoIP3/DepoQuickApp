import { Component } from "@angular/core";
import { TableComponent } from "../../components/table/table.component";
import { DevicesService } from "../../backend/services/devices/devices.service";
import PaginationResponse from "../../backend/services/pagination";
import Device from "../../backend/services/devices/models/device";
import TableColumn from "../../components/table/models/table-column";
import { MessageService } from "primeng/api";
import Pagination from "../../backend/services/pagination";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { ImageGalleryComponent } from "../../components/image-gallery/image-gallery.component";
import { DialogComponent } from "../../components/dialog/dialog.component";
import { DeviceDetailsComponent } from "../device-details/device-details.component";
import { CommonModule } from "@angular/common";
import GetDevicesResponse from "../../backend/services/devices/models/get-devices-response";
import { Subscription } from "rxjs";
import { BusinessesService } from "../../backend/services/businesses/businesses.service";

@Component({
    selector: "app-business-devices-table",
    standalone: true,
    imports: [CommonModule, TableComponent, PaginatorComponent, AvatarComponent, ImageGalleryComponent, DeviceDetailsComponent, DialogComponent],
    templateUrl: "./business-devices-table.component.html"
})
export class BusinessDevicesTableComponent {
    columns: TableColumn[] = [
        { field: "mainPhoto", header: "Photo" },
        { field: "secondaryPhotos", header: "Other Photos" },
        { field: "name", header: "Name" },
        { field: "businessName", header: "Business Name" },
        { field: "type", header: "Type" },
        { field: "modelNumber", header: "Model Number" }
    ];

    devices: Device[] = [];

    loading: boolean = false;
    dialogVisible: boolean = false;
    selectedDevice: Device | null = null;
    pagination: PaginationResponse | null = null;
    private _businessId: string | null = null;
    private _devicesSubscription: Subscription | null = null;

    constructor(
        private readonly _businessesService: BusinessesService,
        private readonly _messageService: MessageService
    ) {
        this._setBusinessId();
    }

    private _setBusinessId(): void {
        const url = window.location.href;
        const urlSegments = url.split("/");
        this._businessId = urlSegments[urlSegments.length - 1];
        console.log(this._businessId);
    }

    ngOnInit() {
        this._subscribeToDevices();
    }

    onPageChange(pagination: Pagination): void {
        this.loading = true;
        this._subscribeToDevices({ ...pagination });
    }

    ngOnDestroy() {
        this._devicesSubscription?.unsubscribe();
    }

    onRowClick(device: Device): void {
        this.selectedDevice = device;
        this.dialogVisible = true;
    }

    closeDialog(): void {
        this.dialogVisible = false;
    }

    private _subscribeToDevices(queries?: object): void {
    this._devicesSubscription = this._businessesService
            .getDevices(this._businessId!,queries ? { ...queries } : {})
            .subscribe({
                next: (response: GetDevicesResponse) => {
                    this.devices = response.devices;
                    this.pagination = response.pagination;
                    this.loading = false;
                },
                error: (error) => {
                    this.loading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

}
