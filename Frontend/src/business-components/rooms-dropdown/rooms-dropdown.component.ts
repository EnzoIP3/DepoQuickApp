import { Component, Input, OnDestroy } from "@angular/core";
import { DropdownComponent } from "../../components/dropdown/dropdown.component";
import { Subscription } from "rxjs";
import { DevicesService } from "../../backend/services/devices/devices.service";
import { RoomsService } from "../../backend/services/rooms/rooms.service";
import AddDeviceToRoomResponse from "../../backend/services/rooms/models/add-device-to-room-response";
import { MessageService } from "primeng/api";
import MoveDeviceResponse from "../../backend/services/devices/models/move-device-response";

@Component({
    selector: "app-rooms-dropdown",
    standalone: true,
    imports: [DropdownComponent],
    templateUrl: "./rooms-dropdown.component.html"
})
export class RoomsDropdownComponent implements OnDestroy {
    @Input() homeId!: string;
    @Input() hardwareId!: string;
    @Input() roomId: string | null = null;
    @Input() rooms: any[] = [];
    @Input() loading = true;

    private _addDeviceToRoomSubscription: Subscription | null = null;
    private _moveDeviceToRoomSubscription: Subscription | null = null;

    constructor(
        private readonly _devicesService: DevicesService,
        private readonly _roomsService: RoomsService,
        private readonly _messageService: MessageService
    ) {}

    ngOnDestroy() {
        this._addDeviceToRoomSubscription?.unsubscribe();
        this._moveDeviceToRoomSubscription?.unsubscribe();
    }

    onRoomChange(roomId: string) {
        if (this.roomId) {
            this._moveDeviceToRoom(roomId);
        } else {
            this._addDeviceToRoom(roomId);
        }
    }

    private _moveDeviceToRoom(roomId: string) {
        this._moveDeviceToRoomSubscription = this._devicesService
            .moveDeviceToRoom(this.hardwareId, { targetRoomId: roomId })
            .subscribe({
                next: (response: MoveDeviceResponse) => {
                    this.roomId = response.targetRoomId;
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Moved device to room"
                    });
                },
                error: (error) => {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    private _addDeviceToRoom(roomId: string) {
        this._addDeviceToRoomSubscription = this._roomsService
            .addDeviceToRoom(roomId, {
                deviceId: this.hardwareId
            })
            .subscribe({
                next: (response: AddDeviceToRoomResponse) => {
                    this.roomId = response.roomId;
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Added device to room"
                    });
                },
                error: (error) => {
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }
}
