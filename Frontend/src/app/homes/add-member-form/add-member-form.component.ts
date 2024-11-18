import { Component, Input } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";

@Component({
    selector: "app-add-member-form",
    templateUrl: "./add-member-form.component.html"
})
export class AddMemberFormComponent {
    readonly formFields = {
        email: {
            required: { message: "Email is required" },
            email: { message: "Email format is invalid" }
        }
    };

    @Input() homeId!: string;
    memberForm!: FormGroup;
    memberStatus = { loading: false };
    private _addMemberSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _homesService: HomesService,
        private _messageService: MessageService
    ) {}

    ngOnInit() {
        this.memberForm = this._formBuilder.group({
            email: ["", [Validators.required, Validators.email]],
            canAddDevices: [false],
            canListDevices: [false],
            canNameDevices: [false]
        });
    }

    onSubmit() {
        this.memberStatus.loading = true;

        this._addMemberSubscription = this._homesService
            .addMember(this.homeId, this.memberForm.value)
            .subscribe({
                next: () => {
                    this.memberStatus.loading = false;
                    this.memberForm.reset({
                        email: "",
                        canAddDevices: false,
                        canListDevices: false,
                        canNameDevices: false
                    });
                    this._messageService.add({
                        severity: "success",
                        summary: "Success",
                        detail: "Member added successfully"
                    });
                },
                error: (error) => {
                    this.memberStatus.loading = false;
                    this._messageService.add({
                        severity: "error",
                        summary: "Error",
                        detail: error.message
                    });
                }
            });
    }

    ngOnDestroy() {
        this._addMemberSubscription?.unsubscribe();
    }
}
