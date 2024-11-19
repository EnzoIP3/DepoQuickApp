import { Component, ContentChild, Input, TemplateRef } from "@angular/core";
import TableColumn from "../../components/table/models/table-column";
import Device from "../../backend/services/devices/models/device";
import { Subscription } from "rxjs";
import { HomesService } from "../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import GetHomeDevicesResponse from "../../backend/services/homes/models/get-home-devices-response";
import { RoomsDropdownComponent } from "../rooms-dropdown/rooms-dropdown.component";
import { BaseDevicesTableComponent } from "../base-devices-table/base-devices-table.component";
import GetRoomsResponse from "../../backend/services/homes/models/get-rooms-response";
import Room from "../../backend/services/homes/models/room";
import { DialogComponent } from "../../components/dialog/dialog.component";
import { CommonModule } from "@angular/common";
import { ButtonComponent } from "../../components/button/button.component";
import { HomeDeviceDetailsComponent } from "../home-device-details/home-device-details.component";

@Component({
    selector: "app-home-devices-table",
    standalone: true,
    imports: [
        RoomsDropdownComponent,
        BaseDevicesTableComponent,
        DialogComponent,
        CommonModule,
        ButtonComponent,
        HomeDeviceDetailsComponent
    ],
    templateUrl: "./home-devices-table.component.html"
})
export class HomeDevicesTableComponent {
    @Input() homeId!: string;
    @ContentChild('nameDeviceFormTemplate') nameDeviceFormTemplate!: TemplateRef<any>;

    columns: TableColumn[] = [
        {
            field: "name",
            header: "Name"
        },
        {
            field: "mainPhoto",
            header: "Main Photo"
        },
        {
            field: "secondaryPhotos",
            header: "Secondary Photos"
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
            field: "details",
            header: "Details"
        }
    ];

    devices: Device[] = [];
    private _roomsSubscription: Subscription | null = null;
    private _getRoomsSubscription: Subscription | null = null;
    private _devicesSubscription: Subscription | null = null;
    private _getDevicesSubscription: Subscription | null = null;
    loading: boolean = true;
    loadingRooms: boolean = true;
    rooms: Room[] = [];
    visibleDialog: boolean = false;
    selectedDevice: Device | null = null;

    constructor(
        private readonly _homesService: HomesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        this._getDevices();
        this._getRooms();
    }

    private _getRooms() {
        this._getRoomsSubscription = this._homesService
            .getRooms(this.homeId)
            .subscribe();
        this._roomsSubscription = this._homesService.rooms.subscribe({
            next: (response: GetRoomsResponse | null) => {
                if (response) {
                    this.rooms = response.rooms;
                    this.loadingRooms = false;
                }
            },
            error: (error) => {
                this.loadingRooms = false;
                this._messageService.add({
                    severity: "error",
                    summary: "Error",
                    detail: error.message
                });
            }
        });
    }

    private _getDevices() {
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

    onRowClick(device: Device) {
        this.visibleDialog = true;
        this.selectedDevice = device;
    }

    ngOnDestroy() {
        this._getRoomsSubscription?.unsubscribe();
        this._roomsSubscription?.unsubscribe();
        this._getDevicesSubscription?.unsubscribe();
        this._devicesSubscription?.unsubscribe();
    }
}
