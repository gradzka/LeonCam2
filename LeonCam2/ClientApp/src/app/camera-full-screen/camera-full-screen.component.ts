import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Camera } from '../shared/models/camera.model';

@Component({
  selector: 'app-camera-full-screen',
  templateUrl: './camera-full-screen.component.html',
  styleUrls: ['./camera-full-screen.component.css']
})
export class CameraFullScreenComponent implements OnInit {
  camera: Camera;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    // TODO
    this.camera = new Camera(Number(this.route.snapshot.paramMap.get('id')), "Baby room full screen", "192.168.1.1");
  }

  removeCamera($event) {
    alert($event.id + $event.name);
    // Delete from db
    // Navigate to dashboard
  }
}
