import { Component, OnInit, OnDestroy, Input } from "@angular/core";
import PaginationResponse from "../../backend/services/pagination";
import Device from "../../backend/services/devices/models/device";
import TableColumn from "../../components/table/models/table-column";
import { MessageService } from "primeng/api";
import Pagination from "../../backend/services/pagination";
import { CommonModule } from "@angular/common";
import GetDevicesResponse from "../../backend/services/devices/models/get-devices-response";
import { Subscription } from "rxjs";
import { BusinessesService } from "../../backend/services/businesses/businesses.service";
import { BaseDevicesTableComponent } from "../base-devices-table/base-devices-table.component";

@Component({
    selector: "app-business-devices-table",
    standalone: true,
    imports: [CommonModule, BaseDevicesTableComponent],
    templateUrl: "./business-devices-table.component.html"
})
export class BusinessDevicesTableComponent implements OnInit, OnDestroy {
    @Input() businessId!: string;

    columns: TableColumn[] = [
        { field: "mainPhoto", header: "Photo" },
        { field: "secondaryPhotos", header: "Other Photos" },
        { field: "name", header: "Name" },
        { field: "businessName", header: "Business Name" },
        { field: "type", header: "Type" },
        { field: "modelNumber", header: "Model Number" }
    ];

    devices: Device[] = [];

    loading = false;
    dialogVisible = false;
    selectedDevice: Device | null = null;
    pagination: PaginationResponse | null = null;
    private _devicesSubscription: Subscription | null = null;

    constructor(
        private readonly _businessesService: BusinessesService,
        private readonly _messageService: MessageService
    ) {}

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
    }

    private _subscribeToDevices(queries?: object): void {
        this._devicesSubscription = this._businessesService
            .getDevices(this.businessId, queries ? { ...queries } : {})
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
