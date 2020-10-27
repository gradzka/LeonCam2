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
  isOn = false;
  loading = false;
  powerOffIcon = faPowerOff;
  trashIcon = faTrash;

  @Input() camera: Camera;
  @Output() removeCamera = new EventEmitter<Camera>();

  constructor(
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  removeCameraCallback = () => {
    this.removeCamera.emit(this.camera);
  }
}
