import { Component } from "@angular/core";
import TableColumn from "../../../components/table/models/table-column";
import TableFilter from "../../../components/table/models/table-filters";

@Component({
    selector: "app-placeholder-home",
    templateUrl: "./placeholder-home.component.html"
})
export class PlaceholderHomeComponent {
    tableColumns: TableColumn[] = [
        { field: "name", header: "Name", filter: true },
        { field: "age", header: "Age", filter: true }
    ];
    tableData = [
        { name: "John", age: 30 },
        { name: "Jane", age: 25 }
    ];
    isLoading = false;

    onFilter(filters: TableFilter[]) {
        console.log("Filter data received:", filters);
    }
}
