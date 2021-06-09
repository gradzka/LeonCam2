import { Component, OnInit, ViewChild } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { faSearch } from '@fortawesome/free-solid-svg-icons';
import { CameraService } from '../_services/camera.service';
import { Camera } from '../shared/models/camera.model';

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

  @ViewChild("addCameraPopover") public addCameraPopover: NgbPopover;

  constructor(
    private formBuilder: FormBuilder,
    private cameraService: CameraService
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
    this.addCameraPopover.close();

    if (this.newCameraForm.invalid) {
      this.addCameraPopover.ngbPopover = "Type valid data";
      this.addCameraPopover.popoverClass = "popover-error-reversed";
      this.addCameraPopover.open();
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
          this.addCameraPopover.ngbPopover = "Success";
          this.addCameraPopover.popoverClass = "popover-success-reversed";
          this.addCameraPopover.open();
          this.addCameraLoading = false;
        },
        error: error => {
          this.addCameraLoading = false;
          this.addCameraPopover.popoverClass = "popover-error-reversed";
          this.addCameraPopover.ngbPopover = error === "Unexpected error" ? "Add New Camera Error" : error;
          this.addCameraPopover.open();
        }
      });

    event.preventDefault();
  }

  filterCameras(event) {
    this.filteredCameras = this.searchedCameras.filter(x => x.toLowerCase().startsWith(event.toLowerCase()));
  }

  getCameras() {
    let cameras =
      ["191.168.1.1", "162.168.1.1", "162.165.1.1", "162.166.1.1", "255.255.255.255",
        "191.168.1.1", "162.168.1.1", "162.165.1.1", "162.166.1.1", "255.255.255.255",
        "191.168.1.1", "162.168.1.1", "162.165.1.1", "162.166.1.1", "255.255.255.255"];

    this.searchedCameras = Array.apply(null, cameras).map((cam, index) => cam);
    this.filteredCameras = this.searchedCameras;
  }

  onSelectCamera(event) {
    this.newCameraControls.ip.setValue(event);
  }

  searchCamerasCallback = () => {
    this.getCameras();
    event.preventDefault();
  }
}
