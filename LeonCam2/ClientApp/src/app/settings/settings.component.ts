import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { UserService } from '@app/_services/user.service';
import { AuthenticationService } from '@app/_services/authentication.service';
import { ToastService } from '../toasts/toast.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit {
  changeUsernameForm: FormGroup;
  changeUsernameLoading = false;

  changePasswordForm: FormGroup;
  changePasswordLoading = false;

  resetAccountForm: FormGroup;
  resetAccountLoading = false;

  deleteAccountForm: FormGroup;
  deleteAccountLoading = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService,
    private toastService: ToastService) {
  }

  ngOnInit() {
    this.changeUsernameForm = this.formBuilder.group({
      changeUsernameNewUsername: ['', Validators.required],
      changeUsernamePassword: ['', Validators.required]
    });
    this.changePasswordForm = this.formBuilder.group({
      changePasswordOldPassword: ['', Validators.required],
      changePasswordNewPassword: ['', Validators.required],
      changePasswordConfirmNewPassword: ['', Validators.required]
    });
    this.resetAccountForm = this.formBuilder.group({
      resetAccountPassword: ['', Validators.required]
    });
    this.deleteAccountForm = this.formBuilder.group({
      deleteAccountPassword: ['', Validators.required]
    });
  }

  get changeUsernameControls() { return this.changeUsernameForm.controls; }
  get changePasswordControls() { return this.changePasswordForm.controls; }
  get resetAccountControls() { return this.resetAccountForm.controls; }
  get deleteAccountControls() { return this.deleteAccountForm.controls; }

  changeUsername(event) {
    if (this.changeUsernameForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    this.changeUsernameLoading = true;
    this.userService.changeUsername(this.changeUsernameControls.changeUsernameNewUsername.value, this.changeUsernameControls.changeUsernamePassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.changeUsernameControls.changeUsernamePassword.setValue("");
          this.toastService.showSuccess("Success");
          this.changeUsernameLoading = false;
        },
        error: error => {
          this.changeUsernameLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Change Username Error" : error);
        }
      });

    event.preventDefault();
  }

  changePassword(event) {
    if (this.changePasswordForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    if (this.changePasswordControls.changePasswordNewPassword.value !== this.changePasswordControls.changePasswordConfirmNewPassword.value) {
      this.toastService.showError("Passwords differ");
      return;
    }

    this.changePasswordLoading = true;
    this.userService.changePassword(this.changePasswordControls.changePasswordOldPassword.value, this.changePasswordControls.changePasswordNewPassword.value, this.changePasswordControls.changePasswordConfirmNewPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.changePasswordControls.changePasswordOldPassword.setValue("");
          this.changePasswordControls.changePasswordNewPassword.setValue("");
          this.changePasswordControls.changePasswordConfirmNewPassword.setValue("");
          this.toastService.showSuccess("Success");
          this.changePasswordLoading = false;
        },
        error: error => {
          this.changePasswordLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Change Password Error" : error);
        }
      });

    event.preventDefault();
  }

  resetAccount(event) {
    if (this.resetAccountForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    this.resetAccountLoading = true;
    this.userService.resetAccount(this.resetAccountControls.resetAccountPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.resetAccountControls.resetAccountPassword.setValue("");
          this.toastService.showSuccess("Success");
          this.resetAccountLoading = false;
        },
        error: error => {
          this.resetAccountLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Reset Account Error" : error);
        }
      });

    event.preventDefault();
  }

  deleteAccount(event) {
    if (this.deleteAccountForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    this.deleteAccountLoading = true;
    this.userService.deleteAccount(this.deleteAccountControls.deleteAccountPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.deleteAccountControls.resetAccountPassword.setValue("");
          this.toastService.showSuccess("Success");
          this.deleteAccountLoading = false;

          this.authenticationService.setCurrentUser(null);
          this.router.navigate(['/']);
        },
        error: error => {
          this.deleteAccountLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Delete Account Error" : error);
        }
      });

    event.preventDefault();
  }
}
