import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { DividerModule } from "primeng/divider";
import ListItem from "./models/list-item";

@Component({
    selector: "app-list",
    standalone: true,
    imports: [CommonModule, DividerModule],
    templateUrl: "./list.component.html"
})
export class ListComponent {
    @Input() items: ListItem[] = [];
}
