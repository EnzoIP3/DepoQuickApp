import { Component, Input } from "@angular/core";
import { ListComponent } from "../../components/list/list.component";
import { CommonModule } from "@angular/common";
import Device from "../../backend/services/devices/models/device";
import ListItem from "../../components/list/models/list-item";
import TableColumn from "../../components/table/models/table-column";

@Component({
    selector: "app-home-device-details",
    standalone: true,
    imports: [ListComponent, CommonModule],
    templateUrl: "./home-device-details.component.html"
})
export class HomeDeviceDetailsComponent {
    @Input() columns!: TableColumn[];
    @Input() device: Device | null = null;

    getExtraFields(device: Device): ListItem[] {
        const formatKey = (key: string): string =>
            key
                .replace(/([a-z])([A-Z])/g, "$1 $2")
                .replace(/_/g, " ")
                .toLowerCase()
                .replace(/^./, (str) => str.toUpperCase());

        const formatValue = (value: any): string | null => {
            if (value === true) return "Yes";
            if (value === false) return "No";
            return value !== null && value !== undefined ? String(value) : null;
        };

        const isTrackedField = (key: string): boolean =>
            this.columns.some((column) => column.field === key);

        const isValidValue = (value: any): boolean =>
            value !== null && value !== undefined;

        return Object.entries(device)
            .filter(
                ([key, value]) => !isTrackedField(key) && isValidValue(value)
            )
            .map(([key, value]) => ({
                label: formatKey(key),
                value: formatValue(value)
            }))
            .filter((item) => item.value !== null);
    }
}
