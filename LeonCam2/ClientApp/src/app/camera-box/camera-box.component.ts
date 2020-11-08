import { Component, OnInit, Output, Input, EventEmitter } from '@angular/core';
import { faCamera, faEdit, faExpandAlt, faPowerOff, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Camera } from '../shared/models/camera.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-camera-box',
  templateUrl: './camera-box.component.html',
  styleUrls: ['./camera-box.component.css']
})
export class CameraBoxComponent implements OnInit {
  cameraIcon = faCamera;
  editIcon = faEdit;
  fullScreenIcon = faExpandAlt;
  isOn = true;
  loading = false;
  powerOffIcon = faPowerOff;
  trashIcon = faTrash;

  @Input() camera: Camera;
  @Input() isFullScreen: boolean = false;
  @Output() removeCamera = new EventEmitter<Camera>();

  constructor(private router: Router) {
  }

  ngOnInit(): void {
  }

  editCameraCallback = () => {
    this.router.navigate(['cameras/edit/' + this.camera.id]);
  }

  fullScreenCallback = () => {
    this.router.navigate(['cameras/fullscreen/' + this.camera.id]);
  }

  isFullScreenRoute() {
    return this.router.url.includes("/fullscreen");
  }

  removeCameraCallback = () => {
    this.removeCamera.emit(this.camera);
  }
}
