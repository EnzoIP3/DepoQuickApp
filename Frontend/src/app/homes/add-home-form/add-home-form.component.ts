import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";
import { MessagesService } from "../../../backend/services/messages/messages.service";

@Component({
    selector: "app-add-home-form",
    templateUrl: "./add-home-form.component.html"
})
export class AddHomeFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        address: {
            required: { message: "Address is required" },
            pattern: {
                message:
                    "Address must include a road and a number (e.g., A Road 123)"
            }
        },
        latitude: {
            required: { message: "Latitude is required" },
            min: { message: "Latitude must be between -90 and 90" },
            max: { message: "Latitude must be between -90 and 90" }
        },
        longitude: {
            required: { message: "Longitude is required" },
            min: { message: "Longitude must be between -90 and 90" },
            max: { message: "Longitude must be between -90 and 90" }
        },
        maxMembers: {
            required: { message: "Max. Members is required" },
            min: { message: "Max. Members must be at least 1" }
        }
    };

    homeForm!: FormGroup;
    homeStatus = { loading: false, error: null };
    private _addHomeSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _homesService: HomesService,
        private _router: Router,
        private _messagesService: MessagesService
    ) {}

    ngOnInit() {
        this.homeForm = this._formBuilder.group({
            address: [
                "",
                [Validators.required, Validators.pattern(/^[A-Za-z\s]+ \d+$/)]
            ],
            latitude: [
                "",
                [Validators.required, Validators.min(-90), Validators.max(90)]
            ],
            longitude: [
                "",
                [Validators.required, Validators.min(-90), Validators.max(90)]
            ],
            maxMembers: ["", [Validators.required, Validators.min(1)]]
        });
    }

    onSubmit() {
        this.homeStatus.loading = true;
        this.homeStatus.error = null;

        this._addHomeSubscription = this._homesService
            .addHome(this.homeForm.value)
            .subscribe({
                next: () => {
                    this.homeStatus.loading = false;
                    this.homeForm.reset();
                    this._messagesService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Home registered successfully"
                    });
                    this._router.navigate(["/homes"]);
                },
                error: (error) => {
                    this.homeStatus.loading = false;
                    this._messagesService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        if (this._addHomeSubscription) {
            this._addHomeSubscription.unsubscribe();
        }
    }
}
