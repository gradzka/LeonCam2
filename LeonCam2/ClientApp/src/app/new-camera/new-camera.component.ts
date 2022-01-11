import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { CameraService } from '../_services/camera.service';
import { Camera } from '../shared/models/camera.model';
import { ToastService } from '../toasts/toast.service';

@Component({
  selector: 'app-new-camera',
  templateUrl: './new-camera.component.html',
  styleUrls: ['./new-camera.component.css']
})

export class NewCameraComponent implements OnInit {
  addCameraLoading = false;
  filteredCameras: string[];
  icon = faSearch;
  newCameraForm: FormGroup;
  searchedCameras: string[];

  constructor(
    private formBuilder: FormBuilder,
    private cameraService: CameraService,
    private toastService: ToastService
  ) { }

  ngOnInit() {
    this.newCameraForm = this.formBuilder.group({
      description: ['', Validators.required],
      ip: ['', Validators.required],
      login: ['', Validators.required],
      password: ['', Validators.required],
    });

    this.newCameraControls.ip.valueChanges.subscribe(value => {
      this.filterCameras(value);
    })

    this.getCameras();
  }

  get newCameraControls() { return this.newCameraForm.controls; }

  addCamera(event) {
    if (this.newCameraForm.invalid) {
      this.toastService.showError("Type valid data");
      return;
    }

    this.addCameraLoading = true;
    this.cameraService.addCamera(
      new Camera(
        -1,
        this.newCameraControls.description.value,
        this.newCameraControls.ip.value,
        this.newCameraControls.login.value,
        this.newCameraControls.password.value))
      .pipe(first())
      .subscribe({
        next: () => {
          this.newCameraControls.description.setValue("");
          this.newCameraControls.ip.setValue("");
          this.newCameraControls.login.setValue("");
          this.newCameraControls.password.setValue("");
          this.toastService.showSuccess("Success");
          this.addCameraLoading = false;
        },
        error: error => {
          this.addCameraLoading = false;
          this.toastService.showError(error === "Unexpected error" ? "Add New Camera Error" : error);
        }
      });

    event.preventDefault();
  }

  filterCameras(event) {
    this.filteredCameras = this.searchedCameras.filter(x => x.toLowerCase().startsWith(event.toLowerCase()));
  }

  getCameras() {
    this.cameraService.discover()
      .pipe(first())
      .subscribe({
        next: data => {
          this.searchedCameras = data;
          this.filteredCameras = this.searchedCameras;

        },
        error: error => {
          this.searchedCameras = null;
          this.filteredCameras = this.searchedCameras;
        }
      });
  }

  onSelectCamera(event) {
    this.newCameraControls.ip.setValue(event);
  }

  searchCamerasCallback = () => {
    this.getCameras();
    event.preventDefault();
  }
}
