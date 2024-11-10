import { Component } from "@angular/core";
import { TableComponent } from "../../components/table/table.component";
import { DevicesService } from "../../backend/services/devices/devices.service";
import DevicesResponse from "../../backend/services/devices/models/devices-response";
import PaginationResponse from "../../backend/services/pagination-response";
import Device from "../../backend/services/devices/models/device";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import { MessageService } from "primeng/api";

@Component({
    selector: "app-devices-table",
    standalone: true,
    imports: [TableComponent],
    templateUrl: "./devices-table.component.html"
})
export class DevicesTableComponent {
    columns: TableColumn[] = [
        {
            field: "name",
            header: "Name",
            filter: true
        },
        {
            field: "businessName",
            header: "Business Name",
            filter: true
        },
        {
            field: "type",
            header: "Type",
            filter: true
        },
        {
            field: "modelNumber",
            header: "Model Number",
            filter: true
        }
    ];

    private _devicesSubscription: Subscription | null = null;

    devices: Device[] = [];
    pagination: PaginationResponse | null = null;
    loading: boolean = true;

    constructor(
        private readonly _devicesService: DevicesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this._devicesSubscription = this._devicesService
            .getDevices()
            .subscribe({
                next: (response: DevicesResponse) => {
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

    ngOnDestroy() {
        this._devicesSubscription?.unsubscribe();
    }
}
