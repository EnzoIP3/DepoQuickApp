import { Component } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { HomeOwnersService } from "../../../backend/services/home-owners/home-owners.service";

@Component({
    selector: "app-register-form",
    templateUrl: "./register-form.component.html"
})
export class RegisterFormComponent {
    readonly formFields = {
        firstName: {
            required: { message: "First name is required" }
        },
        surname: {
            required: { message: "Last name is required" }
        },
        profilePicture: {
            required: { message: "Profile picture URL is required" },
            pattern: { message: "Profile picture must be a valid URL" }
        },
        email: {
            required: { message: "Email is required" },
            email: { message: "Email is invalid" }
        },
        password: {
            required: { message: "Password is required" },
            minlength: {
                message: "Password must be at least 8 characters long"
            }
        }
    };

    registerForm!: FormGroup;
    registerStatus = { loading: false, success: false, error: null };

    constructor(
        private _formBuilder: FormBuilder,
        private _homeOwnersService: HomeOwnersService
    ) {}

    ngOnInit() {
        this.registerForm = this._formBuilder.group({
            name: ["", [Validators.required]],
            surname: ["", [Validators.required]],
            profilePicture: [
                "",
                [
                    Validators.required,
                    Validators.pattern(/^https?:\/\/.*\.(jpg|jpeg|png|gif)$/)
                ]
            ],
            email: ["", [Validators.required, Validators.email]],
            password: ["", [Validators.required, Validators.minLength(8)]]
        });
    }

    onSubmit() {
        this.registerStatus.loading = true;
        this.registerStatus.error = null;
        this.registerStatus.success = false;

        this._homeOwnersService.register(this.registerForm.value).subscribe({
            next: () => {
                this.registerStatus.loading = false;
                this.registerStatus.success = true;
                this.registerForm.reset();
            },
            error: (error) => {
                this.registerStatus.loading = false;
                this.registerStatus.error = error.message;
            }
        });
    }
}
