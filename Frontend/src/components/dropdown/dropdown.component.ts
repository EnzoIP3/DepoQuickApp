import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, Output } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { DropdownModule } from "primeng/dropdown";

@Component({
    selector: "app-dropdown",
    standalone: true,
    imports: [DropdownModule, CommonModule, FormsModule],
    templateUrl: "./dropdown.component.html"
})
export class DropdownComponent {
    @Input() placeholder!: string;
    @Input() showClear = false;
    @Input() options: any[] = [];
    @Input() loading = false;
    @Input() optionLabel = "label";
    @Input() optionValue = "value";
    @Input() value: string | null = null;
    @Output() change = new EventEmitter<any>();
    @Output() click = new EventEmitter<any>();

    onChange(event: any) {
        this.change.emit(event.value);
    }

    onClick(event: any) {
        this.click.emit(event);
    }
}
