import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { DividerModule } from "primeng/divider";
import ListItem from "./models/list-item";
import { SkeletonModule } from "primeng/skeleton";

@Component({
    selector: "app-list",
    standalone: true,
    imports: [CommonModule, DividerModule, SkeletonModule],
    templateUrl: "./list.component.html"
})
export class ListComponent {
    @Input() loading: boolean = false;
    @Input() items: ListItem[] = [];
    loadingArray = new Array(5);
}
