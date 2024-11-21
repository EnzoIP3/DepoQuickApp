import { Component, Input, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Subscription } from "rxjs";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { MessagesService } from "../../../backend/services/messages/messages.service";

@Component({
    selector: "app-add-room-form",
    templateUrl: "./add-room-form.component.html"
})
export class AddRoomFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        name: {
            required: { message: "Name is required" }
        }
    };

    @Input() homeId!: string;
    roomForm!: FormGroup;
    roomStatus = { loading: false };
    private _addRoomSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _homesService: HomesService,
        private _messagesService: MessagesService
    ) {}

    ngOnInit() {
        this.roomForm = this._formBuilder.group({
            name: ["", [Validators.required]]
        });
    }

    onSubmit() {
        this.roomStatus.loading = true;

        this._addRoomSubscription = this._homesService
            .addRoom(this.homeId, this.roomForm.value)
            .subscribe({
                next: () => {
                    this.roomStatus.loading = false;
                    this._messagesService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Added '${this.roomForm.value.name}' as a room`
                    });
                    this.roomForm.reset({
                        name: ""
                    });
                },
                error: (error) => {
                    this.roomStatus.loading = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._addRoomSubscription?.unsubscribe();
    }
}
