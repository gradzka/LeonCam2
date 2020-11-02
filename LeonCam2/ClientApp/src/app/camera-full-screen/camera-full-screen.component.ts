import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-camera-full-screen',
  templateUrl: './camera-full-screen.component.html',
  styleUrls: ['./camera-full-screen.component.css']
})
export class CameraFullScreenComponent implements OnInit {
  cameraId: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.cameraId = this.route.snapshot.paramMap.get('id');
  }

}
