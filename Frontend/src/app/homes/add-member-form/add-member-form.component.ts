import { Component, Input } from "@angular/core";
import { FormBuilder, FormGroup, Validators, FormArray } from "@angular/forms";
import { HomesService } from "../../../backend/services/homes/homes.service";
import { MessageService } from "primeng/api";
import { Subscription } from "rxjs";

@Component({
    selector: "app-add-member-form",
    templateUrl: "./add-member-form.component.html"
})
export class AddMemberFormComponent {
    readonly availablePermissions = [
        { key: "add-devices", label: "Can Add Devices" },
        { key: "get-devices", label: "Can List Devices" },
        { key: "name-device", label: "Can Name Devices" }
    ];

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
            permissions: this._formBuilder.array([])
        });
    }

    get permissions(): FormArray {
        return this.memberForm.get("permissions") as FormArray;
    }

    isPermissionSelected(permission: string): boolean {
        return this.permissions.value.includes(permission);
    }

    togglePermission(permission: string) {
        const index = this.permissions.value.indexOf(permission);
        if (index === -1) {
            this.permissions.push(this._formBuilder.control(permission));
        } else {
            this.permissions.removeAt(index);
        }
    }

    onSubmit() {
        this.memberStatus.loading = true;

        const request = {
            email: this.memberForm.value.email,
            permissions: this.memberForm.value.permissions
        };

        this._addMemberSubscription = this._homesService
            .addMember(this.homeId, request)
            .subscribe({
                next: () => {
                    this.memberStatus.loading = false;
                    this.memberForm.reset({ email: "", permissions: [] });
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
