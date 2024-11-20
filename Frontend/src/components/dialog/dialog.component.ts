import { Component, EventEmitter, Input, Output } from "@angular/core";
import { DialogModule } from "primeng/dialog";

@Component({
    selector: "app-dialog",
    standalone: true,
    imports: [DialogModule],
    templateUrl: "./dialog.component.html",
    styles: ``
})
export class DialogComponent {
    @Input() visible = false;
    @Input() header = "";
    @Input() width = "25rem";
    @Input() closable = true;
    @Input() draggable = false;
    @Input() resizable = false;
    @Input() maximizable = false;
    @Output() visibleChange: EventEmitter<boolean> =
        new EventEmitter<boolean>();

    closeDialog() {
        this.visibleChange.emit(false);
    }
}
