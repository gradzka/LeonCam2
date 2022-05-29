import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { faTimes, faCat } from '@fortawesome/free-solid-svg-icons';

import { AuthenticationService } from '@app/_services/authentication.service';
import { ToastService } from '../toasts/toast.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html'
})

export class RegisterComponent implements OnInit {
  registerForm: FormGroup;
  loading = false;
  faTimes = faTimes;
  mainIcon = faCat;

  @Output() registerClose = new EventEmitter<void>();

  constructor(
    private formBuilder: FormBuilder,
    private authenticationService: AuthenticationService,
    private toastService: ToastService) {
  }

  ngOnInit() {
    this.registerForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      repeatedPassword: ['', Validators.required]
    });
  }

  // convenience getter for easy access to form fields
  get f() { return this.registerForm.controls; }

  onSubmit() {
    if (this.registerForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    if (this.f.password.value !== this.f.repeatedPassword.value) {
      this.toastService.showError("Passwords differ");
      return;
    }

    this.loading = true;
    this.authenticationService.register(this.f.username.value, this.f.password.value, this.f.repeatedPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.f.password.setValue("");
          this.f.repeatedPassword.setValue("");
          this.toastService.showSuccess("Registered");
          this.loading = false;
        },
        error: error => {
          this.loading = false;
          this.toastService.showSuccess(error === "Unexpected error" ? "Sign-Up Error" : error);
        }
      });
  }
}
