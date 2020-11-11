import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { faSearch } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-new-camera',
  templateUrl: './new-camera.component.html',
  styleUrls: ['./new-camera.component.css']
})

export class NewCameraComponent implements OnInit { 
  filteredCameras: string[];
  icon = faSearch;
  newCameraForm: FormGroup;
  searchedCameras: string[];

  constructor(
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.newCameraForm = this.formBuilder.group({
      ip: ['', Validators.required]
    });

    this.newCameraControls.ip.valueChanges.subscribe(value => {
      this.filterCameras(value);
    })

    this.getCameras();
  }

  get newCameraControls() { return this.newCameraForm.controls; }

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
  }
}
