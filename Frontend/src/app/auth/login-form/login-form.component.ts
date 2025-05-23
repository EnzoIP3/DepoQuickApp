import { Component, OnInit, OnDestroy } from "@angular/core";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { AuthService } from "../../../backend/services/auth/auth.service";
import { Router } from "@angular/router";
import { Subscription } from "rxjs";

@Component({
    selector: "app-login-form",
    templateUrl: "./login-form.component.html"
})
export class LoginFormComponent implements OnInit, OnDestroy {
    readonly formFields = {
        password: {
            required: {
                message: "Password is required"
            },
            minlength: {
                message: "Password must be at least 8 characters long"
            }
        },
        email: {
            required: {
                message: "Email is required"
            },
            email: {
                message: "Email is invalid"
            }
        }
    };

    loginForm!: FormGroup;
    loginStatus = { loading: false, error: null };
    private _loginSubscription: Subscription | null = null;

    constructor(
        private _formBuilder: FormBuilder,
        private _authService: AuthService,
        private _router: Router
    ) {}

    ngOnInit() {
        this.loginForm = this._formBuilder.group({
            email: ["", [Validators.required, Validators.email]],
            password: ["", [Validators.required, Validators.minLength(8)]]
        });
    }

    onSubmit() {
        this.loginStatus.loading = true;
        this.loginStatus.error = null;

        this._loginSubscription = this._authService
            .login(this.loginForm.value)
            .subscribe({
                next: () => {
                    this.loginStatus.loading = false;
                    this.loginForm.reset();
                    this._router.navigate(["/devices"]);
                },
                error: (error) => {
                    this.loginStatus.loading = false;
                    this.loginStatus.error = error.message;
                }
            });
    }

    ngOnDestroy() {
        if (this._loginSubscription) {
            this._loginSubscription.unsubscribe();
        }
    }
}
