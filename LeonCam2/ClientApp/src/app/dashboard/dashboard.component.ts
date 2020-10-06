import { Component } from '@angular/core';
import { first } from 'rxjs/operators';

import { UserService } from '@app/_services/user.service';

@Component({ templateUrl: 'dashboard.component.html' })
export class DashboardComponent {
  loading = false;
  users: string;

  constructor(private userService: UserService) { }

  ngOnInit() {
    this.loading = true;
    this.userService.getAll().pipe(first()).subscribe(users => {
      this.loading = false;
      this.users = users;
    });
  }
}
