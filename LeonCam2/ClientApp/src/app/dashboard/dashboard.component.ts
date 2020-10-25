import { Component } from '@angular/core';
import { first } from 'rxjs/operators';
import { Camera } from '../shared/models/camera.model';

//import { UserService } from '@app/_services/user.service';

@Component({ templateUrl: 'dashboard.component.html' })
export class DashboardComponent {
  loading = false;
  cameras : Camera[] = [];
  //cam: string;
  //user: string;

  constructor(/*private userService: UserService*/) { }

  ngOnInit() {
    this.loading = true;
    this.cameras = this.getCameras();
  //  this.userService.getOneStr().pipe(first()).subscribe(
  //    data => {
  //      this.loading = false;
  //      this.cam = data;
  //    });
  //  this.userService.getAll().pipe(first()).subscribe(
  //    data => {
  //      this.loading = false;
  //      this.cameras = data;
  //    });
  //  this.userService.getOne().pipe(first()).subscribe(
  //    data => {
  //      this.loading = false;
  //      this.user = data.username;
  //    });
  }

  getCameras() {
    let cameras = [
      new Camera(1, "Baby room", "192.168.1.1"),
      new Camera(2, "Living room", "192.168.1.2"),
      new Camera(3, "Front door", "192.168.1.3"),
      new Camera(4, "Garage", "192.168.1.4"),
      new Camera(5, "Garden", "192.168.1.5")
    ]

    return Array.apply(null, cameras).map((camera, index) => camera);
  }

}
