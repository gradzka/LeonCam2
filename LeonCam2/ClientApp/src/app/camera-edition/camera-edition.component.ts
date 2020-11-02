import { Component, OnInit, ViewChild } from '@angular/core';
import { faExpandAlt, faPowerOff, faTableTennis, faTh, faTrash } from '@fortawesome/free-solid-svg-icons';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-camera-edition',
  templateUrl: './camera-edition.component.html',
  styleUrls: ['./camera-edition.component.css']
})
export class CameraEditionComponent implements OnInit {
  cameraId: string;
  powerOffIcon = faPowerOff;
  faTableTennisIcon = faTableTennis;
  faExpandAltIcon = faExpandAlt;
  faThIcon = faTh;
  trashIcon = faTrash;

  editCameraForm: FormGroup;
  editCameraLoading = false;
  @ViewChild("editCameraPopover") public editCameraPopover: NgbPopover;

  changePasswordForm: FormGroup;
  changePasswordLoading = false;
  @ViewChild("changePasswordPopover") public changePasswordPopover: NgbPopover;

  constructor(private route: ActivatedRoute, private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.cameraId = this.route.snapshot.paramMap.get('id');

    this.editCameraForm = this.formBuilder.group({
      description: ['', Validators.required],
      ip: ['', Validators.required],
      username: ['', Validators.required]
    });

    this.changePasswordForm = this.formBuilder.group({
      changePasswordOldPassword: ['', Validators.required],
      changePasswordNewPassword: ['', Validators.required],
      changePasswordConfirmNewPassword: ['', Validators.required]
    });
  }

  get changePasswordControls() { return this.changePasswordForm.controls; }
  get editCameraControls() { return this.changePasswordForm.controls; }

  editCamera(event) {
    this.editCameraPopover.close();

    if (this.changePasswordForm.invalid) {
      this.editCameraPopover.ngbPopover = "Type valid data";
      this.editCameraPopover.popoverClass = "popover-error-reversed";
      this.editCameraPopover.open();
      return;
    }

    //IP address regex: TODO

    //if (this.editCameraControls.changePasswordNewPassword.value !== this.editCameraControls.changePasswordConfirmNewPassword.value) {
    //  this.editCameraPopover.ngbPopover = "Passwords differ";
    //  this.editCameraPopover.popoverClass = "popover-error-reversed";
    //  this.editCameraPopover.open();
    //  return;
    //}

    this.editCameraLoading = true;
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
    //this.userService.changePassword(this.changePasswordControls.changePasswordOldPassword.value, this.changePasswordControls.changePasswordNewPassword.value, this.changePasswordControls.changePasswordConfirmNewPassword.value)
    //  .pipe(first())
    //  .subscribe({
    //    next: () => {
    //      this.changePasswordControls.changePasswordOldPassword.setValue("");
    //      this.changePasswordControls.changePasswordNewPassword.setValue("");
    //      this.changePasswordControls.changePasswordConfirmNewPassword.setValue("");
    //      this.changePasswordPopover.ngbPopover = "Success";
    //      this.changePasswordPopover.popoverClass = "popover-success-reversed";
    //      this.changePasswordPopover.open();
    //      this.changePasswordLoading = false;
    //    },
    //    error: error => {
    //      this.changePasswordLoading = false;
    //      this.changePasswordPopover.popoverClass = "popover-error-reversed";
    //      this.changePasswordPopover.ngbPopover = error === "Unexpected error" ? "Change Password Error" : error;
    //      this.changePasswordPopover.open();
    //    }
    //  });

    event.preventDefault();
  }

}
