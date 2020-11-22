import { Component } from '@angular/core';
import { first } from 'rxjs/operators';
import { Camera } from '../shared/models/camera.model';

//import { UserService } from '@app/_services/user.service';

@Component({
  templateUrl: 'dashboard.component.html',
  styleUrls: ['dashboard.component.css', '../../../node_modules/ng-sortgrid/styles/ngsg.css'],
})
export class DashboardComponent {
  cameras: Camera[] = [];
  loading = false;
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
      new Camera(1, "Baby room", "192.168.1.1", "x", "y"),
      new Camera(2, "Living room", "192.168.1.2", "x", "y"),
      new Camera(3, "Front door", "192.168.1.3", "x", "y"),
      new Camera(4, "Garage", "192.168.1.4", "x", "y"),
      new Camera(5, "Garden", "192.168.1.5", "x", "y")
    ]

    return Array.apply(null, cameras).map((camera, index) => camera);
  }

  removeCamera($event) {
    let idx = this.cameras.map(camera => camera.id).indexOf($event.id);
    ~idx && this.cameras.splice(idx, 1);
  }

  storeNewOrder($event) {
  }
}
