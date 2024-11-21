import { Component, OnInit, OnDestroy } from "@angular/core";
import { Subscription } from "rxjs";
import TableColumn from "../../components/table/models/table-column";
import { DeviceTypesService } from "../../backend/services/device-types/device-types.service";
import DeviceTypesResponse from "../../backend/services/device-types/models/device-types-response";
import { TableComponent } from "../../components/table/table.component";
import { MessagesService } from "../../backend/services/messages/messages.service";

@Component({
    selector: "app-device-types-table",
    standalone: true,
    imports: [TableComponent],
    templateUrl: "./device-types-table.component.html"
})
export class DeviceTypesTableComponent implements OnInit, OnDestroy {
    columns: TableColumn[] = [
        {
            field: "type",
            header: "Type"
        }
    ];

    private _deviceTypesSubscription: Subscription | null = null;

    deviceTypes: { type: string }[] = [];
    loading = true;

    constructor(
        private readonly _deviceTypesService: DeviceTypesService,
        private readonly _messagesService: MessagesService
    ) {}

    ngOnInit() {
        this._deviceTypesSubscription = this._deviceTypesService
            .getDeviceTypes()
            .subscribe({
                next: (response: DeviceTypesResponse) => {
                    this.deviceTypes = response.deviceTypes.map(
                        (deviceType) => ({
                            type: deviceType
                        })
                    );
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

    ngOnDestroy() {
        this._deviceTypesSubscription?.unsubscribe();
    }
}
