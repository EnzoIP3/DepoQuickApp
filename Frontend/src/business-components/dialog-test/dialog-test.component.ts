import { Component } from "@angular/core";
import { ButtonComponent } from "../../components/button/button.component";
import { DialogComponent } from "../../components/dialog/dialog.component";

@Component({
    selector: "app-dialog-test",
    standalone: true,
    imports: [ButtonComponent, DialogComponent],
    templateUrl: "./dialog-test.component.html",
    styles: ``
})
export class DialogTestComponent {
    dialogVisible = false;

    showDialog(): void {
        this.dialogVisible = true;
    }

    handleDialogClose(): void {
        console.log("Dialog closed!");
    }

    confirmAction(): void {
        console.log("Action confirmed!");
        this.dialogVisible = false;
    }
}
