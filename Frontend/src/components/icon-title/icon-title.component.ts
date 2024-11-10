import { NgClass } from "@angular/common";
import { Component, Input } from "@angular/core";

@Component({
    selector: "app-icon-title",
    standalone: true,
    imports: [NgClass],
    templateUrl: "./icon-title.component.html"
})
export class IconTitleComponent {
    @Input() icon!: string;
    @Input() title!: string;
}
