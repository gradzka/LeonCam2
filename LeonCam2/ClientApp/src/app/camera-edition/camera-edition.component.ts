import { Component, OnInit } from '@angular/core';
import { faExpandAlt, faPowerOff, faTableTennis, faTh, faTrash } from '@fortawesome/free-solid-svg-icons';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Camera } from '../shared/models/camera.model';
import { CameraService } from '../_services/camera.service';
import { first } from 'rxjs/operators';
import { ToastService } from '../toasts/toast.service';

@Component({
  selector: 'app-camera-edition',
  templateUrl: './camera-edition.component.html'
})
export class CameraEditionComponent implements OnInit {
  camera: Camera;
  faTableTennisIcon = faTableTennis;
  faExpandAltIcon = faExpandAlt;
  faThIcon = faTh;
  isOn = true;
  isOnDashboard = true;
  powerOffIcon = faPowerOff;
  trashIcon = faTrash;

  editCameraForm: FormGroup;
  editCameraLoading = false;

  changePasswordForm: FormGroup;
  changePasswordLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private cameraService: CameraService,
    private toastService: ToastService) { }

  ngOnInit(): void {
    this.editCameraForm = this.formBuilder.group({
      description: ['', Validators.required],
      ip: ['', Validators.required],
      username: ['', Validators.required]
    });

    this.cameraService.get(Number(this.route.snapshot.paramMap.get('id')))
      .pipe(first())
      .subscribe({
        next: data => {
          this.camera = data;
          this.editCameraControls.description.setValue(this.camera.description);
          this.editCameraControls.ip.setValue(this.camera.ip);
          this.editCameraControls.username.setValue(this.camera.login);
        },
        error: error => {
          this.camera = null;
        }
      });    

    this.changePasswordForm = this.formBuilder.group({
      changePasswordOldPassword: ['', Validators.required],
      changePasswordNewPassword: ['', Validators.required],
      changePasswordConfirmNewPassword: ['', Validators.required]
    });
  }

  get changePasswordControls() { return this.changePasswordForm.controls; }
  get editCameraControls() { return this.editCameraForm.controls; }

  editCamera(event) {
    if (this.editCameraForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    //IP address regex: TODO

    //if (this.editCameraControls.changePasswordNewPassword.value !== this.editCameraControls.changePasswordConfirmNewPassword.value) {
    //  this.toastService.showError("Passwords differ");
    //  return;
    //}

    this.editCameraLoading = true;

    this.cameraService.editCamera(
      new Camera(
        this.camera.id,
        this.editCameraControls.description.value,
        this.editCameraControls.ip.value,
        this.editCameraControls.username.value,
        null))
      .pipe(first())
      .subscribe({
        next: () => {
          this.toastService.showSuccess("Success");
          this.editCameraLoading = false;
        },
        error: error => {
          this.editCameraLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Edit Camera Error" : error);
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
    //this.userService.changePassword(this.changePasswordControls.changePasswordOldPassword.value, this.changePasswordControls.changePasswordNewPassword.value, this.changePasswordControls.changePasswordConfirmNewPassword.value)
    //  .pipe(first())
    //  .subscribe({
    //    next: () => {
    //      this.changePasswordControls.changePasswordOldPassword.setValue("");
    //      this.changePasswordControls.changePasswordNewPassword.setValue("");
    //      this.changePasswordControls.changePasswordConfirmNewPassword.setValue("");
    //      this.toastService.showSuccess("Success");
    //      this.changePasswordLoading = false;
    //    },
    //    error: error => {
    //      this.changePasswordLoading = false;
    //      this.toastService.showError(error === "Unexpected error" ? "Change Password Error" : error);
    //    }
    //  });

    event.preventDefault();
  }

  fullScreenCallback = () => {
    this.router.navigate(['cameras/fullscreen/' + this.camera.id]);
  }

  removeCameraCallback = () => {
    alert(this.camera.id);
    // Delete from db
    // Navigate to dashboard
  }
}
