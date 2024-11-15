import { Component } from "@angular/core";
import { TableComponent } from "../../components/table/table.component";
import { DevicesService } from "../../backend/services/devices/devices.service";
import GetDevicesResponse from "../../backend/services/devices/models/get-devices-response";
import PaginationResponse from "../../backend/services/pagination";
import Device from "../../backend/services/devices/models/device";
import TableColumn from "../../components/table/models/table-column";
import { Subscription } from "rxjs";
import { MessageService } from "primeng/api";
import Pagination from "../../backend/services/pagination";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { ImageGalleryComponent } from "../../components/image-gallery/image-gallery.component";
import { DialogComponent } from "../../components/dialog/dialog.component";
import { DeviceDetailsComponent } from "../device-details/device-details.component";
import { CommonModule } from "@angular/common";

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

    // Datos simulados directamente, sin usar array
    devices: Device[] = [
        {
            id: "1",
            mainPhoto: "https://picsum.photos/200",
            secondaryPhotos: [
                "https://picsum.photos/200",
                "https://picsum.photos/200"
            ],
            name: "Device 1",
            businessName: "Business 1",
            type: "Type A",
            modelNumber: "Model 001"
        },
        {
            id: "2",
            mainPhoto: "https://picsum.photos/200",
            secondaryPhotos: [
                "https://picsum.photos/200",
                "https://picsum.photos/200"
            ],
            name: "Device 2",
            businessName: "Business 2",
            type: "Type B",
            modelNumber: "Model 002"
        },
        {
            id: "3",
            mainPhoto: "https://picsum.photos/200",
            secondaryPhotos: [
                "https://picsum.photos/200",
                "https://picsum.photos/200"
            ],
            name: "Device 3",
            businessName: "Business 3",
            type: "Type C",
            modelNumber: "Model 003"
        },
        {
            id: "4",
            mainPhoto: "https://picsum.photos/200",
            secondaryPhotos: [
                "https://picsum.photos/200",
                "https://picsum.photos/200"
            ],
            name: "Device 4",
            businessName: "Business 4",
            type: "Type D",
            modelNumber: "Model 004"
        },
        {
            id: "5",
            mainPhoto: "https://picsum.photos/2003",
            secondaryPhotos: [
                "https://picsum.photos/2004",
                "https://picsum.photos/2005"
            ],
            name: "Device 5",
            businessName: "Business 5",
            type: "Type E",
            modelNumber: "Model 005"
        }
    ];

    pagination: PaginationResponse | null = {
        page: 1,
        pageSize: 10,
        totalPages: 5,
    };

    loading: boolean = false;
    dialogVisible: boolean = false;
    selectedDevice: Device | null = null;

    constructor(
        private readonly _devicesService: DevicesService,
        private readonly _messageService: MessageService
    ) {}

    ngOnInit() {
        // Cargar datos de prueba cuando la componente se inicialice
        this._loadDummyData();
    }

    // Método de paginación
    onPageChange(pagination: Pagination): void {
        console.log("Pagina cambiada: ", pagination);
        this.loading = true;
        this._loadDummyData(pagination);
    }

    ngOnDestroy() {
        // Limpiar recursos si es necesario
    }

    private _loadDummyData(pagination: Pagination = { page: 1, pageSize: 10 }): void {
        console.log("Obteniendo dispositivos con paginación: ", pagination);
        this.loading = false;
        console.log("Dispositivos simulados: ", this.devices);
        console.log("Paginación simulada: ", this.pagination);
    }

    onRowClick(device: Device): void {
        this.selectedDevice = device;
        this.dialogVisible = true;
        console.log("Dispositivo seleccionado: ", device);
    }

    closeDialog(): void {
        this.dialogVisible = false;
        console.log("Cerrando diálogo");
    }

}
