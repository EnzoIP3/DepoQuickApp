import { Component, Input } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import Device from "../../backend/services/devices/models/device";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import GetHomeDevicesResponse from "../../backend/services/homes/models/get-home-devices-response";
import { RoomsDropdownComponent } from "../rooms-dropdown/rooms-dropdown.component";
import { BaseDevicesTableComponent } from "../base-devices-table/base-devices-table.component";

@Component({
    selector: "app-home-devices-table",
    standalone: true,
    imports: [RoomsDropdownComponent, BaseDevicesTableComponent],
    templateUrl: "./home-devices-table.component.html"
})
export class HomeDevicesTableComponent {
    @Input() homeId!: string;

    columns: TableColumn[] = [
        {
            field: "mainPhoto",
            header: "Main Photo"
        },
        {
            field: "secondaryPhotos",
            header: "Secondary Photos"
        },
        {
            field: "name",
            header: "Name"
        },
        {
            field: "hardwareId",
            header: "Hardware ID"
        },
        {
            field: "type",
            header: "Type"
        },
        {
            field: "roomId",
            header: "Room"
        },
        {
            field: "extraFields",
            header: "Extra Fields"
        }
    ];

    getExtraFields(device: Device): string {
        const formatKey = (key: string): string =>
            key
                .replace(/([a-z])([A-Z])/g, "$1 $2")
                .replace(/_/g, " ")
                .toLowerCase()
                .replace(/^./, (str) => str.toUpperCase());

        const formatValue = (value: any): string =>
            value === true ? "Yes" : value === false ? "No" : value;

        return Object.entries(device)
            .filter(
                ([key, value]) =>
                    !this.columns.find((column) => column.field === key) &&
                    value !== null &&
                    value !== undefined
            )
            .map(([key, value]) => `${formatKey(key)}: ${formatValue(value)}`)
            .join(", ");
    }

    devices: Device[] = [];
    private _devicesSubscription: Subscription | null = null;
    private _getDevicesSubscription: Subscription | null = null;
    loading: boolean = true;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this._getDevicesSubscription = this._homesService
            .getDevices(this.homeId)
            .subscribe();
        this._devicesSubscription = this._homesService.devices.subscribe({
            next: (response: GetHomeDevicesResponse | null) => {
                if (response) {
                    this.devices = response.devices;
                }
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

    ngOnDestroy() {
        this._getDevicesSubscription?.unsubscribe();
        this._devicesSubscription?.unsubscribe();
    }
}
