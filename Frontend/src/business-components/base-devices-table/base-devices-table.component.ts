import { Component, EventEmitter, Input, Output } from "@angular/core";
import Device from "../../backend/services/devices/models/device";
import PaginationResponse from "../../backend/services/pagination";
import { TableComponent } from "../../components/table/table.component";
import { PaginatorComponent } from "../../components/paginator/paginator.component";
import { DialogComponent } from "../../components/dialog/dialog.component";
import { DeviceDetailsComponent } from "../device-details/device-details.component";
import { CommonModule } from "@angular/common";
import TableColumn from "../../components/table/models/table-column";
import Pagination from "../../backend/services/pagination";
import { AvatarComponent } from "../../components/avatar/avatar.component";
import { ImageGalleryComponent } from "../../components/image-gallery/image-gallery.component";
import { FilterValues } from "../../components/table/models/filter-values";

@Component({
    selector: "app-base-devices-table",
    templateUrl: "./base-devices-table.component.html",
    standalone: true,
    imports: [
        TableComponent,
        PaginatorComponent,
        DialogComponent,
        DeviceDetailsComponent,
        CommonModule,
        AvatarComponent,
        ImageGalleryComponent
    ]
})
export class BaseDevicesTableComponent {
    @Input() columns: TableColumn[] = [];
    @Input() filterableColumns: any[] = [];
    @Input() devices: Device[] = [];
    @Input() paginate = true;
    @Input() pagination: PaginationResponse | null = null;
    @Input() loading = false;
    @Input() clickableRows = true;
    @Input() customTemplates: Record<string, any> = {};
    @Input() selectedDevice: Device | null = null;
    @Output() rowClick: EventEmitter<Device> = new EventEmitter<Device>();
    @Output() pageChange: EventEmitter<Pagination> =
        new EventEmitter<Pagination>();
    @Output() filterChange: EventEmitter<FilterValues> =
        new EventEmitter<FilterValues>();
    @Input() showDialog = true;
    dialogVisible = false;

    onRowClick(device: Device): void {
        if (this.showDialog) {
            this.dialogVisible = true;
        }
        this.rowClick.emit(device);
    }

    onFilterChange(filterValues: FilterValues): void {
        this.filterChange.emit(filterValues);
    }

    closeDialog(): void {
        this.dialogVisible = false;
    }

    onPageChange(pagination: any): void {
        this.pageChange.emit(pagination);
    }

    appendTemplates(
        templates: Record<string, any>,
        customTemplates: Record<string, any>
    ): Record<string, any> {
        return Object.assign({}, templates, customTemplates);
    }
}
