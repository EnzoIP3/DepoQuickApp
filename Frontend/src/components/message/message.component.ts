import { Component, Input, OnInit } from "@angular/core";
import { Message } from "primeng/api";
import { MessagesModule } from "primeng/messages";

@Component({
    selector: "app-message",
    standalone: true,
    imports: [MessagesModule],
    templateUrl: "./message.component.html"
})
export class MessageComponent implements OnInit {
    @Input() text: string | undefined;
    @Input() severity: "success" | "info" | "warn" | "error" | undefined;
    messages: Message[] = [];

    ngOnInit() {
        this.messages = [
            {
                severity: this.severity || "info",
                summary: this.text || ""
            }
        ];
    }
}
