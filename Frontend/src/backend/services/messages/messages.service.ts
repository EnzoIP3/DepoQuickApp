import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";
import Message from "./models/message";

@Injectable({
    providedIn: "root"
})
export class MessagesService {
    constructor(private readonly _service: MessageService) {}

    public add(message: Message): void {
        this._service.add(message);
    }
}
