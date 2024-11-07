import { Component, EventEmitter, Input, Output } from "@angular/core";
import { ButtonModule } from "primeng/button";
import { ToolbarModule } from "primeng/toolbar";

@Component({
    selector: "app-toolbar",
    standalone: true,
    imports: [ToolbarModule, ButtonModule],
    templateUrl: "./toolbar.component.html"
})
export class ToolbarComponent {
    @Input() title: string | undefined;
    @Output() onClick = new EventEmitter<void>();

    handleClick() {
        this.onClick.emit();
    }
}
