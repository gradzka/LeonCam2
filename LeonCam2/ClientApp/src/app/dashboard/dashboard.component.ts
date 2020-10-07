import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { UserService } from '@app/_services/user.service';

@Component({ templateUrl: 'dashboard.component.html' })
export class DashboardComponent {
  loading = false;
  cameras: string[];
  cam: string;
  user: string;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.loading = true;
    this.userService.getOneStr().pipe(first()).subscribe(
      data => {
        this.loading = false;
        this.cam = data;
      });
    this.userService.getAll().pipe(first()).subscribe(
      data => {
        this.loading = false;
        this.cameras = data;
      });
    this.userService.getOne().pipe(first()).subscribe(
      data => {
        this.loading = false;
        this.user = data.username;
      });
  }
}
