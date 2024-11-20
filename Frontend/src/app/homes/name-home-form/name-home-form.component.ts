import { Component, Input } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";
import { HomesService } from "../../../backend/services/homes/homes.service";

@Component({
    selector: "app-name-home-form",
    templateUrl: "./name-home-form.component.html"
})
export class NameHomeFormComponent {
    readonly formFields = {
        newName: {
            required: { message: "Name is required" }
        }
    };

    @Input() homeId!: string;
    homeForm!: FormGroup;
    homeStatus = { loading: false };
    private _nameHomeSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _homesService: HomesService,
        private _messageService: MessageService
    ) {}

    ngOnInit() {
        this.homeForm = this._formBuilder.group({
            newName: ["", [Validators.required]]
        });
    }

    onSubmit() {
        this.homeStatus.loading = true;

        this._nameHomeSubscription = this._homesService
            .nameHome(this.homeId, this.homeForm.value)
            .subscribe({
                next: () => {
                    this.homeStatus.loading = false;
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: `Home is now named ${this.homeForm.value.newName}`
                    });
                    this.homeForm.reset({
                        name: ""
                    });
                },
                error: (error) => {
                    this.homeStatus.loading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._nameHomeSubscription?.unsubscribe();
    }
}
