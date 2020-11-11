import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { UserService } from '@app/_services/user.service';
import { AuthenticationService } from '@app/_services/authentication.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html'
})
export class SettingsComponent implements OnInit {
  changeUsernameForm: FormGroup;
  changeUsernameLoading = false;
  @ViewChild("changeUsernamePopover") public changeUsernamePopover: NgbPopover;

  changePasswordForm: FormGroup;
  changePasswordLoading = false;
  @ViewChild("changePasswordPopover") public changePasswordPopover: NgbPopover;

  resetAccountForm: FormGroup;
  resetAccountLoading = false;
  @ViewChild("resetAccountPopover") public resetAccountPopover: NgbPopover;

  deleteAccountForm: FormGroup;
  deleteAccountLoading = false;
  @ViewChild("deleteAccountPopover") public deleteAccountPopover: NgbPopover;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private authenticationService: AuthenticationService,
    private userService: UserService) {
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
    this.changeUsernamePopover.close();

    if (this.changeUsernameForm.invalid) {
      this.changeUsernamePopover.ngbPopover = "Type valid data2";
      this.changeUsernamePopover.popoverClass = "popover-error-reversed";
      this.changeUsernamePopover.open();
      return;
    }

    this.changeUsernameLoading = true;
    this.userService.changeUsername(this.changeUsernameControls.changeUsernameNewUsername.value, this.changeUsernameControls.changeUsernamePassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.changeUsernameControls.changeUsernamePassword.setValue("");
          this.changeUsernamePopover.ngbPopover = "Success";
          this.changeUsernamePopover.popoverClass = "popover-success-reversed";
          this.changeUsernamePopover.open();
          this.changeUsernameLoading = false;
        },
        error: error => {
          this.changeUsernameLoading = false;
          this.changeUsernamePopover.popoverClass = "popover-error-reversed";
          this.changeUsernamePopover.ngbPopover = error === "Unexpected error" ? "Change Username Error" : error;
          this.changeUsernamePopover.open();
        }
      });

    event.preventDefault();
  }

  changePassword(event) {
    this.changePasswordPopover.close();

    if (this.changePasswordForm.invalid) {
      this.changePasswordPopover.ngbPopover = "Type valid data";
      this.changePasswordPopover.popoverClass = "popover-error-reversed";
      this.changePasswordPopover.open();
      return;
    }

    if (this.changePasswordControls.changePasswordNewPassword.value !== this.changePasswordControls.changePasswordConfirmNewPassword.value) {
      this.changePasswordPopover.ngbPopover = "Passwords differ";
      this.changePasswordPopover.popoverClass = "popover-error-reversed";
      this.changePasswordPopover.open();
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
          this.changePasswordPopover.ngbPopover = "Success";
          this.changePasswordPopover.popoverClass = "popover-success-reversed";
          this.changePasswordPopover.open();
          this.changePasswordLoading = false;
        },
        error: error => {
          this.changePasswordLoading = false;
          this.changePasswordPopover.popoverClass = "popover-error-reversed";
          this.changePasswordPopover.ngbPopover = error === "Unexpected error" ? "Change Password Error" : error;
          this.changePasswordPopover.open();
        }
      });

    event.preventDefault();
  }

  resetAccount(event) {
    this.resetAccountPopover.close();

    if (this.resetAccountForm.invalid) {
      this.resetAccountPopover.ngbPopover = "Type valid data";
      this.resetAccountPopover.popoverClass = "popover-error-reversed";
      this.resetAccountPopover.open();
      return;
    }

    this.resetAccountLoading = true;
    this.userService.resetAccount(this.resetAccountControls.resetAccountPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.resetAccountControls.resetAccountPassword.setValue("");
          this.resetAccountPopover.ngbPopover = "Success";
          this.resetAccountPopover.popoverClass = "popover-success-reversed";
          this.resetAccountPopover.open();
          this.resetAccountLoading = false;
        },
        error: error => {
          this.resetAccountLoading = false;
          this.resetAccountPopover.popoverClass = "popover-error-reversed";
          this.resetAccountPopover.ngbPopover = error === "Unexpected error" ? "Reset Account Error" : error;
          this.resetAccountPopover.open();
        }
      });

    event.preventDefault();
  }

  deleteAccount(event) {
    this.deleteAccountPopover.close();

    if (this.deleteAccountForm.invalid) {
      this.deleteAccountPopover.ngbPopover = "Type valid data";
      this.deleteAccountPopover.popoverClass = "popover-error-reversed";
      this.deleteAccountPopover.open();
      return;
    }

    this.deleteAccountLoading = true;
    this.userService.deleteAccount(this.deleteAccountControls.deleteAccountPassword.value)
      .pipe(first())
      .subscribe({
        next: () => {
          this.deleteAccountControls.resetAccountPassword.setValue("");
          this.deleteAccountPopover.ngbPopover = "Success";
          this.deleteAccountPopover.popoverClass = "popover-success-reversed";
          this.deleteAccountPopover.open();
          this.deleteAccountLoading = false;

          this.authenticationService.setCurrentUser(null);
          this.router.navigate(['/']);
        },
        error: error => {
          this.deleteAccountLoading = false;
          this.deleteAccountPopover.popoverClass = "popover-error-reversed";
          this.deleteAccountPopover.ngbPopover = error === "Unexpected error" ? "Delete Account Error" : error;
          this.deleteAccountPopover.open();
        }
      });

    event.preventDefault();
  }
}
