import { Component, EventEmitter, Input, Output } from "@angular/core";
import { DialogModule } from "primeng/dialog";

@Component({
    selector: "app-dialog",
    standalone: true,
    imports: [DialogModule],
    templateUrl: "./dialog.component.html"
})
export class DialogComponent {
    @Input() title: string = "";
    @Input() visible: boolean = false;
    @Input() header: string = "";
    @Input() width: string = "25rem";
    @Input() closable: boolean = true;
    @Input() draggable: boolean = false;
    @Input() resizable: boolean = false;
    @Input() maximizable: boolean = false;
    @Output() visibleChange: EventEmitter<boolean> =
        new EventEmitter<boolean>();

    closeDialog() {
        this.visibleChange.emit(false);
    }
}
