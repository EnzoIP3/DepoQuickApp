import { Component, OnInit, OnDestroy } from "@angular/core";
import { DevicesService } from "../../backend/services/devices/devices.service";
import GetDevicesResponse from "../../backend/services/devices/models/get-devices-response";
import PaginationResponse from "../../backend/services/pagination";
import Device from "../../backend/services/devices/models/device";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import Pagination from "../../backend/services/pagination";
import { FilterValues } from "../../components/table/models/filter-values";
import { CommonModule } from "@angular/common";
import { BaseDevicesTableComponent } from "../base-devices-table/base-devices-table.component";
import { MessagesService } from "../../backend/services/messages/messages.service";

@Component({
    selector: "app-devices-table",
    standalone: true,
    imports: [CommonModule, BaseDevicesTableComponent],
    templateUrl: "./devices-table.component.html"
})
export class DevicesTableComponent implements OnInit, OnDestroy {
    columns: TableColumn[] = [
        {
            field: "mainPhoto",
            header: "Photo"
        },
        {
            field: "secondaryPhotos",
            header: "Other Photos"
        },
        {
            field: "name",
            header: "Name"
        },
        {
            field: "businessName",
            header: "Business Name"
        },
        {
            field: "type",
            header: "Type"
        },
        {
            field: "modelNumber",
            header: "Model Number"
        }
    ];

    filterableColumns: string[] = [
        "name",
        "type",
        "modelNumber",
        "businessName"
    ];

    private _devicesSubscription: Subscription | null = null;

    devices: Device[] = [];
    pagination: PaginationResponse | null = null;
    filters: FilterValues = {};
    loading = true;
    selectedDevice: Device | null = null;
    dialogVisible = false;

    constructor(
        private readonly _devicesService: DevicesService,
        private readonly _messagesService: MessagesService
    ) {}

    ngOnInit() {
        this._subscribeToDevices();
    }

    onPageChange(pagination: Pagination): void {
        this.loading = true;
        this._subscribeToDevices({ ...pagination, ...this.filters });
    }

    onFilterChange(filters: FilterValues) {
        this.filters = filters;
        this._subscribeToDevices({ ...this.pagination, ...filters });
    }

    ngOnDestroy() {
        this._devicesSubscription?.unsubscribe();
    }

    private _subscribeToDevices(queries?: object): void {
        this._devicesSubscription = this._devicesService
            .getDevices(queries ? { ...queries } : {})
            .subscribe({
                next: (response: GetDevicesResponse) => {
                    this.devices = response.devices;
                    this.pagination = response.pagination;
                    this.loading = false;
                },
                error: (error) => {
                    this.loading = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    onRowClick(device: Device): void {
        this.selectedDevice = device;
    }
}
